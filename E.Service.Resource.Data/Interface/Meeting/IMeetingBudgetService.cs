using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting
{
    public interface IMeetingBudgetService
    {
        Task<MeetingBudget> Save(MeetingBudget entity);

        Task<Control<MeetingBudget>> Get(int start, int take, string filter, string order,bool showActive);

        Task<MeetingBudget> Get(int id);
    }
}
