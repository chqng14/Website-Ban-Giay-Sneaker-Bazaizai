using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.SanPhamChiTiet.ThuongHieuDTO;
using App_Data.ViewModels.XuatXu;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XuatXuController : ControllerBase
    {
        private readonly IAllRepo<XuatXu> _xuaXuRespo;
        private readonly IMapper _mapper;
        private readonly BazaizaiContext _bazaizaiContext;

        public XuatXuController(IAllRepo<XuatXu> xuaXuRespo, IMapper mapper)
        {
            _xuaXuRespo = xuaXuRespo;
            _mapper = mapper;
            _bazaizaiContext = new BazaizaiContext();
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
            try
            {
                var xuaXu = GetXuatXu(id);
                if (xuaXu != null)
                {
                    return _xuaXuRespo.RemoveItem(xuaXu);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPut("sua-xuat-xu")]
        public bool Update(XuatXuDTO xuaXuDTO)
        {
            try
            {
                var nameXuatXu = xuaXuDTO.Ten!.Trim().ToLower();
                if (!_bazaizaiContext.XuatXus.Where(x => x.Ten!.Trim().ToLower() == nameXuatXu).Any())
                {
                    var xuatXu = _mapper.Map<XuatXu>(xuaXuDTO);
                    _bazaizaiContext.Attach(xuatXu);
                    _bazaizaiContext.Entry(xuatXu).Property(sp => sp.Ten).IsModified = true;
                    _bazaizaiContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
