using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class MauSac
    {
        [Key]
        public Guid IDMauSac { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
