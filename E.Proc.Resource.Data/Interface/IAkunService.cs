using E.Proc.Resource.Data.Interface.DTO;
using E.Proc.Resource.Data.List;
using E.Proc.Resource.Data.Model;
using System.Collections.Generic;

namespace E.Proc.Resource.Data.Interface
{
    public interface IAkunService
    {
        //MasterAkun Save(MasterAkun entity);

        Control<MasterAkun> Get(int start, int take, string filter, string order);
        IEnumerable<MasterAkun> GetKodeAkun(int start, int take, string filter);
        IEnumerable<MasterAkun> GetKodeAkunBiaya(int start, int take, string filter);

        IEnumerable<AkunDTO> Get(int id);

        IEnumerable<AkunDTO> GetAkunDepartment(int departmentId);
        IEnumerable<MasterAkun> GetKodeAkunElement(int start, int take, string filter);
        MstAnggaranGagas GetBiaya(string masterAkun, string biayaEL, int tahun);
        MasterAkun GetKodeELementBiaya(int id);
        MasterAkun GetKodeAkunId(int id);
    }
}
