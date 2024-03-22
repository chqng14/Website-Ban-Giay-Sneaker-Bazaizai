using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2013.PowerPoint;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static App_Data.Repositories.TrangThai;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {      
        private readonly IAllRepo<NguoiDung> repos;
        BazaizaiContext context = new BazaizaiContext();
        DbSet<NguoiDung> nguoiDungs;
        public NguoiDungController()
        {
            nguoiDungs = context.NguoiDungs;
            AllRepo<NguoiDung> all = new AllRepo<NguoiDung>(context, nguoiDungs);
            repos = all;
        }
        [HttpPut("ChinhSuaTongTien")]
        public bool UpdateTongTien(string IdNguoiDung,double? Tien)
        {
           var NguoiDung=  repos.GetAll().First(p => p.Id == IdNguoiDung);
            if (NguoiDung != null)
            {
                NguoiDung.TongChiTieu +=Tien;
                return repos.EditItem(NguoiDung);
            }
            else return false;
        }
    }
}
