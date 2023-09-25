using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.XuatXu;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XuatXuController : ControllerBase
    {
        private readonly IAllRepo<XuatXu> _xuaXuRespo;
        private readonly IMapper _mapper;

        public XuatXuController(IAllRepo<XuatXu> xuaXuRespo, IMapper mapper)
        {
            _xuaXuRespo = xuaXuRespo;
            _mapper = mapper;
        }


        [HttpGet("GetXuatXu/{id}")]
        public XuatXu? GetXuatXu(string id)
        {
            return _xuaXuRespo.GetAll().FirstOrDefault(x => x.IdXuatXu == id);
        }


        [HttpGet("GetAllXuatXu")]
        public List<XuatXu> GetListXuatXu()
        {
            return _xuaXuRespo.GetAll().Where(x => x.TrangThai == 0).ToList();
        }

        [HttpPost("CreateXuatXu")]
        public bool Create(XuatXuDTO xuaXuDTO)
        {
            xuaXuDTO.IdXuatXu = Guid.NewGuid().ToString();
            var xuaXu = _mapper.Map<XuatXu>(xuaXuDTO);
            xuaXu.TrangThai = 0;
            xuaXu.Ma = _xuaXuRespo.GetAll().Count() == 0 ? "XX1" : "XX" + (_xuaXuRespo.GetAll().Count() + 1);
            return _xuaXuRespo.AddItem(xuaXu);
        }

        [HttpDelete("DeleteXuatXu/{id}")]
        public bool Delete(string id)
        {
            var xuaXu = GetXuatXu(id);
            if (xuaXu != null)
            {
                xuaXu!.TrangThai = 1;
                return _xuaXuRespo.EditItem(xuaXu);
            }
            return false;

        }

        [HttpPut("UpdateXuatXu")]
        public bool Update(XuatXuDTO xuaXuDTO)
        {
            var xuaXuGet = GetXuatXu(xuaXuDTO.IdXuatXu);
            if (xuaXuGet != null)
            {
                _mapper.Map(xuaXuDTO, xuaXuGet);
                return _xuaXuRespo.EditItem(xuaXuGet);
            }
            return false;
        }
    }
}
