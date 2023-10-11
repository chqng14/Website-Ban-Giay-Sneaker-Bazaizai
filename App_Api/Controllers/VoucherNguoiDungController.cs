using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.Voucher;
using App_Data.ViewModels.VoucherNguoiDung;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static App_Data.Repositories.TrangThai;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherNguoiDungController : ControllerBase
    {
        private readonly IAllRepo<VoucherNguoiDung> VcNguoiDungRepos;
        private readonly IAllRepo<Voucher> VcRepos;
        private readonly IMapper _mapper;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        DbSet<Voucher> voucher;
        public VoucherNguoiDungController(IMapper mapper)
        {
            voucherNguoiDung = DbContextModel.voucherNguoiDungs;

            AllRepo<VoucherNguoiDung> VcNd = new AllRepo<VoucherNguoiDung>(DbContextModel, voucherNguoiDung);
            VcNguoiDungRepos = VcNd;

            AllRepo<Voucher> Vc = new AllRepo<Voucher>(DbContextModel, voucher);
            VcRepos = Vc;

            _mapper = mapper;
        }
        // GET: api/<ChatLieuController>
        [HttpGet("GetAllVoucherNguoiDung")]
        public async Task<IEnumerable<VoucherNguoiDungDTO>> GetAllVouCherNguoiDung()
        {
            var lstVoucherNguoiDung = voucherNguoiDung.Include(x => x.Vouchers).ToList();
            var lstVoucherNguoiDungDTO = _mapper.Map<IEnumerable<VoucherNguoiDungDTO>>(lstVoucherNguoiDung);

            return lstVoucherNguoiDungDTO;
        }

        [HttpGet("GetAllVoucherNguoiDungByID{id}")]
        public IEnumerable<VoucherNguoiDungDTO> GetAllVoucherNguoiDungByID(string id)
        {
            var lstVoucherNguoiDung = voucherNguoiDung.Include(x => x.Vouchers).ToList();
            var lstVoucherNguoiDungDTO = _mapper.Map<IEnumerable<VoucherNguoiDungDTO>>(lstVoucherNguoiDung).Where(c => c.IdNguoiDung == id);
            return lstVoucherNguoiDungDTO;
        }

        // GET api/<ChatLieuController>/5
        [HttpGet("GetChiTietVoucherNguoiDungByID{id}")]
        public VoucherNguoiDung GetVoucherNguoiDungById(string id)
        {
            return VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCherNguoiDung == id);
        }
        // POST api/<ChatLieuController>
        [HttpPost("AddVoucherNguoiDung")]
        public bool AddVoucherNguoiDung(VoucherNguoiDungDTO VcDTO)
        {
            var VoucherKhaDung = VcRepos.GetAll().FirstOrDefault(c => c.IdVoucher == VcDTO.IdVouCher);
            if (VoucherKhaDung != null)
            {
                if (VoucherKhaDung.TrangThai == (int)TrangThai.TrangThaiVoucher.HoatDong)
                {
                    VcDTO.IdVouCherNguoiDung = Guid.NewGuid().ToString();
                    var voucherND = _mapper.Map<VoucherNguoiDung>(VcDTO);
                    return VcNguoiDungRepos.AddItem(voucherND);
                }
            }
            return false;
        }
        // PUT api/<ChatLieuController>/5
        [HttpPut("UpdateVoucherNguoiDung{id}")]
        public bool UpdateVoucherNguoiDungSauKhiDung(VoucherNguoiDungDTO VcDTO)
        {
            var voucherGet = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCher == VcDTO.IdVouCher);
            if (voucherGet != null)
            {
                _mapper.Map(VcDTO, voucherGet);
                voucherGet.TrangThai = (int)TrangThai.TrangThaiVoucherNguoiDung.DaSuDung;
                return VcNguoiDungRepos.EditItem(voucherGet);
            }
            return false;
        }
        // DELETE api/<ChatLieuController>/5
        [HttpDelete("XoaVoucherNguoiDung{id}")]
        public bool Delete(string id)
        {
            var cl = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCherNguoiDung == id);
            return VcNguoiDungRepos.RemoveItem(cl);
        }
    }
}
