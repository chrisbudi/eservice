using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum EJenisRole
    {
        [Description("JenisCetakan")]
        JENISCETAKAN,
        [Description("JenisPrasarana")]
        JENISPRASARANA,
        [Description("JenisMeeting")]
        JENISMEETING,
        [Description("JenisBarang")]
        JENISBARANG,
        [Description("JenisKondisiAsset")]
        JENISKONDISIASSET,
        [Description("JenisSarana")]
        JENISSARANA,
        [Description("JenisKepemilikan")]
        JENISKEPEMILIKAN,
        [Description("JenisPembayaran")]
        JENISPEMBAYARAN,
        [Description("JenisATK")]
        JENISATK,
        [Description("JenisMobil")]
        JENISMOBIL


    }
}
