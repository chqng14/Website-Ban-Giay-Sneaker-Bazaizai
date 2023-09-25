using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.MauSac;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauSacController : ControllerBase
    {
        private readonly IAllRepo<MauSac> _mauSacRespo;
        private readonly IMapper _mapper;

        public MauSacController(IAllRepo<MauSac> mauSacRespo, IMapper mapper)
        {
            _mauSacRespo = mauSacRespo;
            _mapper = mapper;
        }


        [HttpGet("GetMauSac/{id}")]
        public MauSac? GetMauSac(string id)
        {
            return _mauSacRespo.GetAll().FirstOrDefault(x => x.IdMauSac == id);
        }


        [HttpGet("GetAllMauSac")]
        public List<MauSac> GetListMauSac()
        {
            return _mauSacRespo.GetAll().Where(x => x.TrangThai == 0).ToList();
        }

        [HttpPost("CreateMauSac")]
        public bool Create(MauSacDTO mauSacDTO)
        {
            mauSacDTO.IdMauSac = Guid.NewGuid().ToString();
            var mauSac = _mapper.Map<MauSac>(mauSacDTO);
            mauSac.TrangThai = 0;
            mauSac.MaMauSac =  !_mauSacRespo.GetAll().Any()? "MS1" : "MS" + (_mauSacRespo.GetAll().Count() + 1);
            return _mauSacRespo.AddItem(mauSac);
        }

        [HttpDelete("DeleteMauSac/{id}")]
        public bool Delete(string id)
        {
            var mauSac = GetMauSac(id);
            if(mauSac != null)
            {
                mauSac!.TrangThai = 1;
                return _mauSacRespo.EditItem(mauSac);
            }
            return false;
        }

        [HttpPut("UpdateMauSac")]
        public bool Update(MauSacDTO mauSacDTO)
        {
            var mauSacGet = GetMauSac(mauSacDTO.IdMauSac);
            if (mauSacGet != null)
            {
                _mapper.Map(mauSacDTO,mauSacGet);
                return _mauSacRespo.EditItem(mauSacGet);
            }
            return false;
        }

    }
}
