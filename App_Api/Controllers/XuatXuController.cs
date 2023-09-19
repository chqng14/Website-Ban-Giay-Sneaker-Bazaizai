using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XuatXuController : ControllerBase
    {
        private readonly IAllRepo<XuatXu> _xuatXuRespo;

        public XuatXuController(IAllRepo<XuatXu> xuatXuRespo)
        {
            _xuatXuRespo = xuatXuRespo;
        }


        [HttpGet("GetXuatXu/{id}")]
        public XuatXu? GetXuatXu(string id)
        {
            return _xuatXuRespo.GetAll().FirstOrDefault(x => x.IdXuatXu == id);
        }


        [HttpGet("GetAllXuatXu")]
        public List<XuatXu> GetListXuatXu()
        {
            return _xuatXuRespo.GetAll().Where(x => x.TrangThai > 0).ToList();
        }

        [HttpPost("CreateXuatXu")]
        public bool Create(XuatXu xuatXu)
        {
            xuatXu.Ma = _xuatXuRespo.GetAll().Count() == null? "XX1" : "XX" + (_xuatXuRespo.GetAll().Count() + 1);
            return _xuatXuRespo.AddItem(xuatXu);
        }

        [HttpDelete("DeleteXuatXu/{id}")]
        public bool Delete(XuatXu xuatXu)
        {
            xuatXu.TrangThai = 0;
            return _xuatXuRespo.EditItem(xuatXu);
        }

        [HttpDelete("UpdateXuatXu/{id}")]
        public bool Update(XuatXu xuatXu)
        {
            var xuatXuGet = GetXuatXu(xuatXu.IdXuatXu);
            if (xuatXuGet != null)
            {
                return _xuatXuRespo.AddItem(xuatXu);
            }
            return false;
        }
    }
}
