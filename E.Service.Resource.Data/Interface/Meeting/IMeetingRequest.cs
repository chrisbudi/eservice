using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Interface.Meeting.DTO.Transaction;
using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting
{
    public interface IMeetingRequest
    {

        /// <summary>
        /// save entity with submit option
        /// </summary>
        /// <param name="request">request entity bind</param>
        /// <param name="submit">true or false submit option</param>
        /// <returns>meeting request object if valid, null if not falid</returns>
        Task<MeetingRequests> Save(MeetingRequests request, bool submit, int pusatBiaya);

        /// <summary>
        /// Update entity to get anggaran id
        /// </summary>
        /// <param name="entityId">meeting request id</param>
        /// <param name="anggaranId"></param>
        /// <returns></returns>
        Task<MeetingRequests> UpdateEntity(int entityId, int anggaranId);

        Task<IList<MeetingBudget>> MeetingButget(int start, int take, string filter);

        Task<Control<MeetingRequestDTO>> GetAccountabilityRequestList(int start, int take, string filter, string order);


        Task<Control<MeetingRequestDTO>> GetList(int start, int take, string filter, string order);

        Task<MeetingRequestDTO> Get(int id);

        Task<MeetingRequestAccountability> SaveAccountablity(MeetingRequestAccountability meeting, bool submit);

        Task<IList<MeetingRoomsDTO>> Rooms(MeetingRequestRoomBinding meetingRequestBinding);

        Task<Control<MeetingRequestAccountabilityDTO>> GetAccountablityList(int start, int take, string filter, string order);

        Task<MeetingRequestAccountabilityDTO> GetAccountablity(int id);
        Task<IList<MeetingRequestTimeDTO>> GetRequestRoomId(MeetingRequestRoomId meetingrequestRoomId);
        Task<IList<MeetingRequestTime>> GetRoomValidate(DateTime startTime, DateTime endTime, int roomId);
        Task<IList<MeetingRequestDTO>> GetRoomId(int id);
        Task<MeetingRequests> GetMeetingRequestId(int requestId);
        Task<MeetingRequestAccountabilityDTO> GetByRequestConfirmId(int requestId);
        Task<MeetingRequestDTO> GetByRequestId(int requestId);
    }
}
