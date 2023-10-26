using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietDTO
{
    public class SanPhamChiTietCopyDTO
    {
        public SanPhamChiTietDTO? SanPhamChiTietData { get; set; } = new SanPhamChiTietDTO();
        public List<string>? ListTenAnhRemove { get; set; }
        public List<IFormFile>? ListAnhCreate { get; set; }

    }
}
