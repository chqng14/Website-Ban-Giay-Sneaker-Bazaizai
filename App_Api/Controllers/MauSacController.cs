using App_Data.IRepositories;
using App_Data.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauSacController : ControllerBase
    {
        private readonly IAllRepo<MauSac> _mauSacRespo;

        public MauSacController(IAllRepo<MauSac> mauSacRespo)
        {
            _mauSacRespo = mauSacRespo;
        }


        [HttpGet("GetMauSac/{id}")]
        public MauSac? GetMauSac(string id)
        {
            return _mauSacRespo.GetAll().FirstOrDefault(x => x.IdMauSac == id);
        }


        [HttpGet("GetAllMauSac")]
        public List<MauSac> GetListMauSac()
        {
            return _mauSacRespo.GetAll().Where(x=>x.TrangThai>0).ToList();
        }

        [HttpPost("CreateMauSac")]
        public bool Create(MauSac mauSac)
        {
            mauSac.MaMauSac = _mauSacRespo.GetAll().Count() == null ? "MM1" : "MM" + (_mauSacRespo.GetAll().Count() + 1);
            return _mauSacRespo.AddItem(mauSac);
        }

        [HttpDelete("DeleteMauSac/{id}")]
        public bool Delete(MauSac mauSac)
        {
            mauSac.TrangThai = 0;
            return _mauSacRespo.EditItem(mauSac);
        }

        [HttpDelete("UpdateMauSac/{id}")]
        public bool Update(MauSac mauSac)
        {
            var mauSacGet = GetMauSac(mauSac.IdMauSac);
            if(mauSacGet != null)
            {
                return _mauSacRespo.AddItem(mauSac);
            }
            return false;
        }

    }
}
