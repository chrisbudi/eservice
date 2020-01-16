using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core
{
    public interface IJenisRoleService
    {
        Task<Jenis> Save(Jenis entity);

        Task<Control<JenisRoleDTO>> Get(int start, int take, string filter, string order, bool showActive, string roleId);

        Task<JenisRoleDTO> Get(int id);
        Task<string> GetRoleId(EJenisRole jenisRole);
        Task<Control<JenisRoleDTO>> GetName(int start, int take, string filter, 
            string order, bool showActive, EJenisRole jenisRole);
    }
}
