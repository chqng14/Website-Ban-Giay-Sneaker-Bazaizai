using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTiet
{
    public class ResponseCheckAddOrUpdate
    {
        public bool Success { get; set; }
        public SanPhamChiTietDTO? Data { get; set; }
    }
}
