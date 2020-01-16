using E.Service.Resource.Api.Services.Core;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services
{
    public class RequestService : IRequestService
    {
        private EservicesdbContext db;
        private IEmailSender emailService;

        public RequestService(EservicesdbContext db, IEmailSender emailService)
        {
            this.db = db;
            this.emailService = emailService;
        }



        public async Task<int> BeginStateId(ERequestType request)
        {
            var processName = request.Description();

            var flowType = ETransitionType.Next.Description();


            var data = await db.Transition.Include(m => m.Process)
                .OrderBy(m => m.Transitionid)
                .FirstOrDefaultAsync(m =>
                    m.Flowtype == flowType &&
                    m.Process.Nama == processName &&
                    m.TransitionEnd == false);
            if (data != null)
                return data.Currentstateid.Value;
            else
            {
                var dataTransition = db.Transition.Include(m => m.Process)
                        .OrderBy(m => m.Transitionid)
                        .Where(m =>
                           m.Flowtype == flowType &&
                           m.Process.Nama == processName);
                if (dataTransition.Count() == 1)
                {
                    return dataTransition.Single().Currentstateid.Value;
                }
                else
                {
                    throw new Exception("Not found data");
                }
            }

        }

        public async Task<int> ProgressId(ERequestType procees)
        {
            string processName = procees.Description();
            var data = await db.Process.SingleAsync(m => m.Nama == processName);

            return data.Processid;
        }

        public async Task<List<Actions>> ActionState(EActionType actiontype, ERequestType procees)
        {
            int actionTypeId = (int)actiontype;
            int processId = (int)procees;

            var data = db.Actions
                .OrderBy(m => m.Actionid).Where(m => m.Actiontypeid == actionTypeId && m.Processid == processId);

            return await data.ToListAsync();
        }

        public async Task<List<Transition>> TransitionList(ERequestType procees)
        {
            int processId = (int)procees;
            var data = db.Transition.Include(m => m.Transitionaction)
                .OrderBy(m => m.Transitionid)
                .Where(m => m.Processid == processId);

            return await data.ToListAsync();
        }

        public async Task<Transition> TransitionState(int stateId, ETransitionType transitionType, int processId)
        {
            string transition = transitionType.Description();

            var data = await db.Transition.Include(m => m.Transitionaction)
                .OrderBy(m => m.Transitionid)
                .SingleAsync(m =>
                    m.Currentstateid == stateId &&
                    m.Processid == processId &&
                    m.Flowtype == transition);

            return data;
        }

        public async Task<RequestFlow> RequestFlow(int transactionId)
        {
            var data = await db.RequestFlow
                .Include(m => m.Process)
                .OrderBy(m => m.Requestid)
                .SingleAsync(m =>
                    m.Requestid == transactionId);

            return data;
        }

        public async Task<Requestaction> RequestAction(int requestId, int transitionId, int actionId)
        {
            var data = await db.Requestaction
                .OrderBy(m => m.Requestactionid)
                .SingleAsync(m =>
                    m.Requestid == requestId &&
                    m.Transitionid == transitionId &&
                    m.Actionid == actionId);

            return data;
        }

        public async Task<string> GetRequestCurrentState(int requestId)
        {
            var flow = await db.RequestFlow
              .Include(m => m.Currentstate)
              .SingleAsync(m => m.Requestid == requestId);

            return flow.Currentstate.Name;
        }

        public async Task<Requestaction> SetStateRequest(int requestId, ETransitionType transitionType, RequestActionHistory requestActionHistory)
        {
            var request = await RequestFlow(requestId);
            int nextTransitionActionId = 0;

            var defaultStateIdforReject = request.Currentstateid;

            var processId = request.Processid;

            var transition = await TransitionState(request.Currentstateid,
                transitionType,
                processId);

            request.Currentstateid = transition.Nextstateid ?? 0;
            var currentRequestAction = await RequestAction(
                request.Requestid,
                transition.Transitionid,
                transition.Transitionaction.Actonid);


            currentRequestAction.Iscomplete = true;
            currentRequestAction.Isactive = false;

            if (transition.TransitionEnd != true && transitionType != ETransitionType.Cancel)
            {
                Transition nextTransition;

                if (transitionType != ETransitionType.Reject)
                {
                    nextTransition = await TransitionState(transition.Nextstateid.Value,
                    transitionType, processId);
                }
                else
                {
                    nextTransition = await TransitionState(transition.Nextstateid.Value,
                    ETransitionType.Next, processId);
                }

                var nextRequestAction = await RequestAction(
                    request.Requestid,
                    nextTransition.Transitionid,
                    nextTransition.Transitionaction.Actonid);

                nextTransitionActionId = nextRequestAction.Actionid;

                nextRequestAction.Iscomplete = false;
                nextRequestAction.Isactive = true;
                if (transition.TransitionEnd == true)
                    nextRequestAction.Isactive = false;

                request.Requestaction.Add(nextRequestAction);

                if (transitionType == ETransitionType.Reject)
                {
                    Transition nextTransitionTODisable = await TransitionState(defaultStateIdforReject,
                    ETransitionType.Next, processId);

                    var nextRequestActionToDisable = await RequestAction(
                    request.Requestid,
                    nextTransitionTODisable.Transitionid,
                    nextTransitionTODisable.Transitionaction.Actonid);


                    nextRequestActionToDisable.Iscomplete = false;
                    nextRequestActionToDisable.Isactive = false;
                    request.Requestaction.Add(nextRequestActionToDisable);
                }
            }
            request.Requestaction.Add(currentRequestAction);
            try

            {
                db.RequestFlow.Update(request);
                await db.SaveChangesAsync();


                requestActionHistory.RequestId = currentRequestAction.Requestid;
                requestActionHistory.RequestActionId = currentRequestAction.Requestactionid;

                if (transitionType == ETransitionType.Next)
                {
                    await SetRequestActionHistory(requestActionHistory, EHistoryType.Approve);
                    if (nextTransitionActionId != 0 || currentRequestAction.Transition.TransitionEnd == true)
                        await emailService.SendEmailNext(int.Parse(request.Userid), nextTransitionActionId, request, requestActionHistory.UserId, currentRequestAction.Transition);
                }
                else if (transitionType == ETransitionType.Cancel)
                {

                    await SetRequestActionHistory(requestActionHistory, EHistoryType.Cancel);
                    if (nextTransitionActionId != 0 || currentRequestAction.Transition.TransitionEnd == true)
                        await emailService.SendEmailCancel(int.Parse(request.Userid), request.Processid, request, requestActionHistory.UserId);

                }
                else if (transitionType == ETransitionType.Reject)
                {

                    await SetRequestActionHistory(requestActionHistory, EHistoryType.Reject);


                    if (nextTransitionActionId != 0 || currentRequestAction.Transition.TransitionEnd == true)
                        await emailService.SendEmailReject(int.Parse(request.Userid), requestActionHistory.UserId, request);


                }

                return currentRequestAction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString(), ex.InnerException);
            }
        }

        public async Task<RequestActionHistory> SetRequestActionHistory(RequestActionHistory entity, EHistoryType historyType)
        {
            entity.Datetime = DateTime.Now;
            entity.HistoryType = historyType.Description();
            entity.Note = historyType.Description();
            await db.RequestActionHistory.AddAsync(entity);
            await db.SaveChangesAsync();



            var request = db.RequestFlow.SingleAsync(m => m.Requestid == entity.RequestId);

            //email to pic
            //await emailService.SendEmailReject(int.Parse(entity.Request.Userid), entity.UserId, entity.Request);

            return entity;
        }
    }
}
