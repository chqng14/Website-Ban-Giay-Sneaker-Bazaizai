using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.Voucher;
using App_Data.ViewModels.VoucherNguoiDung;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.AspNetCore.Identity;
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
        private readonly IVoucherNguoiDungRepos voucherNguoiDungRep;
        private readonly IAllRepo<Voucher> VcRepos;
        private readonly IMapper _mapper;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        DbSet<Voucher> voucher;
        public VoucherNguoiDungController(IMapper mapper)
        {
            voucherNguoiDung = DbContextModel.VoucherNguoiDungs;
            voucher = DbContextModel.Vouchers;
            AllRepo<VoucherNguoiDung> VcNd = new AllRepo<VoucherNguoiDung>(DbContextModel, voucherNguoiDung);
            VcNguoiDungRepos = VcNd;
            voucherNguoiDungRep = new VoucherNguoiDungRepos();
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
        public bool AddVoucherNguoiDung(string MaVoucher, string idNguoiDung)
        {
            var VoucherKhaDung = VcRepos.GetAll().FirstOrDefault(c => c.MaVoucher == MaVoucher && c.TrangThai == (int)TrangThaiVoucher.HoatDong && c.SoLuong > 0);
            if (VoucherKhaDung != null)
            {
                var existsInVoucherNguoiDung = VcNguoiDungRepos.GetAll().Any(vnd => vnd.IdVouCher == VoucherKhaDung.IdVoucher && vnd.IdNguoiDung == idNguoiDung);

                if (existsInVoucherNguoiDung)
                {
                    // IdVoucher đã tồn tại trong bảng VoucherNguoiDung
                    return false;
                }
                else
                {
                    VoucherNguoiDung VCNguoiDung = new VoucherNguoiDung()
                    {
                        IdVouCherNguoiDung = Guid.NewGuid().ToString(),
                        IdNguoiDung = idNguoiDung,
                        IdVouCher = VoucherKhaDung.IdVoucher,
                        TrangThai = (int)TrangThaiVoucherNguoiDung.KhaDung
                        ,
                        NgayNhan = DateTime.Now
                    };
                    if (VcNguoiDungRepos.AddItem(VCNguoiDung) == true)
                    {
                        VoucherKhaDung.SoLuong = VoucherKhaDung.SoLuong - 1;
                        VcRepos.EditItem(VoucherKhaDung);
                    }
                    return true; // Trả về true nếu thành công
                }
            }

            return false;
        }

        // PUT api/<ChatLieuController>/5
        [HttpPut("UpdateVoucherNguoiDung{id}")]
        public bool UpdateVoucherNguoiDungsauKhiDung(VoucherNguoiDungDTO VcDTO)
        {
            var voucherGet = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCherNguoiDung == VcDTO.IdVouCherNguoiDung);
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


        [HttpPost("AddVoucherNguoiDungTuAdmin")]
        public async Task<string> AddVoucherNguoiDungTuAdmin(AddVoucherRequestDTO addVoucherRequestDTO)
        {
            var VoucherKhaDung = VcRepos.GetAll().FirstOrDefault(c => c.MaVoucher == addVoucherRequestDTO.MaVoucher && c.TrangThai == (int)TrangThaiVoucher.HoatDong);
            if (VoucherKhaDung != null)
            {
                foreach (var item in addVoucherRequestDTO.UserId.ToList())
                {
                    var existsInVoucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(vnd => vnd.IdVouCher == VoucherKhaDung.IdVoucher && vnd.IdNguoiDung == item);

                    if (existsInVoucherNguoiDung != null)
                    {
                        // IdVoucher đã tồn tại trong bảng VoucherNguoiDung, loại bỏ phần tử khỏi danh sách UserId
                        addVoucherRequestDTO.UserId.Remove(item);
                    }
                }
                int soNguoiDuocTangVoucher = addVoucherRequestDTO.UserId.Count();
                if (soNguoiDuocTangVoucher > 0 && VoucherKhaDung.SoLuong >= soNguoiDuocTangVoucher)
                {
                    if (await (voucherNguoiDungRep.TangVoucherNguoiDung(addVoucherRequestDTO)) == true)
                    {
                        return "Tặng voucher thành công";
                    }
                }
                else
                {
                    return "Người dùng đã có voucher này rồi";
                }

            }
            return "Voucher không còn khả dụng";
        }
        [HttpPost("TangVoucherChoNguoiDungMoi")]
        public async Task<bool> TangVoucherChoNguoiDungMoi(string ma)
        {
            int i = 0;
            var lstNguoidungNew = await voucherNguoiDungRep.GetLstNguoiDungMoi();
            if (lstNguoidungNew.Any())
            {
                foreach (var item in lstNguoidungNew)
                {
                    if (await voucherNguoiDungRep.TangVoucherNguoiDungMoi(ma, item.Id) == true)
                    {
                        i++;
                    }
                }
                if (i > 0)
                    return true;
            }
            return false;

        }
        [HttpGet("GetVocherTaiQuay")]
        public async Task<VoucherTaiQuayDto> GetVocherTaiQuay(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            return await voucherNguoiDungRep.GetVocherTaiQuay(id);
        }

    }
}
