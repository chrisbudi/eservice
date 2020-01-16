using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface
{
    public interface IRequestService
    {
        Task<int> BeginStateId(ERequestType request);
        Task<int> ProgressId(ERequestType request);
        Task<List<Actions>> ActionState(EActionType actiontype, ERequestType procees);
        Task<List<Transition>> TransitionList(ERequestType procees);
        Task<Transition> TransitionState(int stateId, ETransitionType transitionType, int processId);
        Task<RequestFlow> RequestFlow(int transactionId);
        Task<Requestaction> RequestAction(int requestId, int transitionId, int actionId);

        Task<string> GetRequestCurrentState(int requestId);
        Task<Requestaction> SetStateRequest(int requestId, ETransitionType transitionType, RequestActionHistory requestActionHistory);

        Task<RequestActionHistory> SetRequestActionHistory(RequestActionHistory entity, EHistoryType historyType);
    }
}
