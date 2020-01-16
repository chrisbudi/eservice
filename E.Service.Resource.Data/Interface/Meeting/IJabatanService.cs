using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting
{
    public interface IJabatanService
    {
        Task<Jabatan> Save(Jabatan entity);

        Task<Control<Jabatan>> Get(int start, int take, string filter, string order, bool showActive);

        Task<JabatanDTO> Get(int id);

        Task<JabatanDTO> SaveChild(JabatanDTO entity);
    }
}
