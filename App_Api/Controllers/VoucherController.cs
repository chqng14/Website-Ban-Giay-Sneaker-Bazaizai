using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.Voucher;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using static App_Data.Repositories.TrangThai;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IAllRepo<VoucherNguoiDung> VcNguoiDungRepos;
        private readonly IAllRepo<Voucher> allRepo;
        private readonly IMapper _mapper;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<Voucher> Vouchers;
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        public VoucherController(IMapper mapper)
        {
            voucherNguoiDung = DbContextModel.VoucherNguoiDungs;
            AllRepo<VoucherNguoiDung> VcNd = new AllRepo<VoucherNguoiDung>(DbContextModel, voucherNguoiDung);
            VcNguoiDungRepos = VcNd;
            Vouchers = DbContextModel.Vouchers;
            AllRepo<Voucher> all = new AllRepo<Voucher>(DbContextModel, Vouchers);
            allRepo = all;
            _mapper = mapper;
        }
        [HttpGet("GetVoucher")]
        public List<Voucher> GetAllVoucher()
        {
            return allRepo.GetAll().ToList();
        }
        [HttpGet("GetVoucherByMa/{ma}")]
        public Voucher? GetVoucher(string ma)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.MaVoucher == ma);
        }
        [HttpGet("GetVoucherDTOByMa/{id}")]
        public VoucherDTO? GetVoucherDTO(string id)
        {
            var Voucher = allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == id);
            var VoucherDTO = _mapper.Map<VoucherDTO>(Voucher);
            return VoucherDTO;
        }
        private string GenerateRandomVoucherCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            List<Voucher> Vouchers = GetAllVoucher(); // Lấy danh sách các mã khuyến mãi hiện có
            bool isDuplicate = false; // Biến kiểm tra trùng lặp
            string voucherCode = ""; // Biến lưu mã khuyến mãi ngẫu nhiên
            do
            {
                voucherCode = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[random.Next(s.Length)]).ToArray()); // Tạo mã khuyến mãi ngẫu nhiên
                isDuplicate = Vouchers.Any(v => v.MaVoucher == voucherCode); // Kiểm tra xem mã khuyến mãi có trùng với mã nào trong danh sách không
            } while (isDuplicate); // Nếu trùng thì lặp lại quá trình tạo và kiểm tra

            return voucherCode; // Trả về mã khuyến mãi không trùng
        }
        #region VoucherOn
        [HttpPost("CreateVoucher")]
        public bool Create(VoucherDTO voucherDTO)
        {
            voucherDTO.IdVoucher = Guid.NewGuid().ToString();
            var voucher = _mapper.Map<Voucher>(voucherDTO);
            voucher.MaVoucher = GenerateRandomVoucherCode();
            voucher.NgayTao = DateTime.Now;
            if (voucher.NgayBatDau > DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.ChuaBatDau;
            }
            if (voucher.NgayBatDau <= DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.HoatDong;
            }
            if (voucher.SoLuong == 0)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.KhongHoatDong;
            }
            if (voucher.SoLuong > 0 && voucher.NgayBatDau < DateTime.Now && voucher.NgayKetThuc > DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.HoatDong;
            }
            return allRepo.AddItem(voucher);
        }
        [HttpPut("DeleteVoucher/{id}")]
        public bool Delete(string id)
        {
            var VoucherGet = GetAllVoucher().FirstOrDefault(c => c.IdVoucher == id);
            if (VoucherGet != null)
            {
                if (VoucherGet.TrangThai != (int)TrangThaiVoucher.DaHuy)
                {
                    VoucherGet!.TrangThai = (int)TrangThaiVoucher.DaHuy;
                    allRepo.EditItem(VoucherGet);
                    //sau khi huỷ hoạt động voucher sẽ xoá voucher người dùng khi họ chưa dùng
                    var VoucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCher == id && c.TrangThai != (int)TrangThaiVoucherNguoiDung.DaSuDung);
                    return VcNguoiDungRepos.RemoveItem(VoucherNguoiDung);
                }
            }
            return false;
        }
        [HttpPut("RestoreVoucher/{id}")]
        public bool RestoreVoucher(string id)
        {
            var VoucherGet = GetAllVoucher().FirstOrDefault(c => c.IdVoucher == id);
            if (VoucherGet != null)
            {
                if (VoucherGet.NgayBatDau < DateTime.Now && VoucherGet.NgayKetThuc > DateTime.Now)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.HoatDong;
                }
                else if (VoucherGet.NgayBatDau > DateTime.Now && VoucherGet.NgayKetThuc > DateTime.Now)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.ChuaBatDau;
                }
                else
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.KhongHoatDong;
                }
                return allRepo.EditItem(VoucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucher")]
        public bool Update(VoucherDTO voucherDTO)
        {
            var voucherGet = allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == voucherDTO.IdVoucher);
            DateTime NgayTao = voucherGet.NgayTao;
            if (voucherGet != null)
            {
                voucherDTO.MaVoucher = voucherGet.MaVoucher;
                _mapper.Map(voucherDTO, voucherGet);
                if (voucherDTO.TrangThai == null)
                {

                }
                if (voucherGet.NgayBatDau > DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.ChuaBatDau;
                }
                if (voucherGet.NgayBatDau < DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.HoatDong; ;
                }
                if (voucherGet.SoLuong == 0)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.KhongHoatDong;
                }
                if (voucherGet.SoLuong > 0 && voucherGet.NgayBatDau < DateTime.Now && voucherGet.NgayKetThuc > DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.HoatDong;
                }
                voucherGet.NgayTao = NgayTao;
                return allRepo.EditItem(voucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucherAfterUseIt/{idVoucher}/{idUser}")]
        public bool UpdateVoucherAfterUseIt(string idVoucher, string idUser)
        {
            var voucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCher == idVoucher && c.IdNguoiDung == idUser);
            if (voucherNguoiDung != null)
            {
                voucherNguoiDung.TrangThai = (int)TrangThaiVoucherNguoiDung.DaSuDung;
                VcNguoiDungRepos.EditItem(voucherNguoiDung);
                return true;
            }
            return false;
        }
        [HttpPut("UpdateVoucher/{idVoucher}")]
        //trừ số lượng
        public bool UpdateVoucher(string idVoucher)
        {
            var voucher = allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == idVoucher);
            if (voucher != null)
            {
                voucher.SoLuong -= 1;
                allRepo.EditItem(voucher);
                return true;
            }
            return false;
        }
        #endregion

        #region VoucherTaiQuay
        [HttpGet("GetVoucherTaiQuay")]
        public List<Voucher> GetAllVoucherTaiQuay()
        {
            return allRepo.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucher.HoatDongTaiQuay).ToList();
        }
        [HttpPost("CreateVoucherTaiQuay")]
        public bool CreateTaiQuay(VoucherDTO voucherDTO)
        {
            voucherDTO.IdVoucher = Guid.NewGuid().ToString();
            var voucher = _mapper.Map<Voucher>(voucherDTO);
            voucher.MaVoucher = GenerateRandomVoucherCode();
            voucher.NgayTao = DateTime.Now;
            voucher.SoLuong = 0;
            if (voucher.NgayBatDau > DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.ChuaHoatDongTaiQuay;
            }
            if (voucher.NgayBatDau <= DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.HoatDongTaiQuay;
            }
            if (voucher.SoLuong > 0 && voucher.NgayBatDau < DateTime.Now && voucher.NgayKetThuc > DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.HoatDongTaiQuay;
            }
            return allRepo.AddItem(voucher);
        }

        [HttpPut("DeleteVoucherTaiQuay/{id}")]
        public bool DeleteTaiQuay(string id)
        {
            var VoucherGet = GetAllVoucher().FirstOrDefault(c => c.IdVoucher == id);
            if (VoucherGet != null)
            {
                if (VoucherGet.TrangThai != (int)TrangThaiVoucher.DaHuyTaiQuay)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.DaHuyTaiQuay;
                    allRepo.EditItem(VoucherGet);
                    //sau khi huỷ hoạt động voucher sẽ xoá voucher người dùng khi họ chưa dùng
                    var VoucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCher == id && c.TrangThai != (int)TrangThaiVoucherNguoiDung.DaSuDung);
                    return VcNguoiDungRepos.RemoveItem(VoucherNguoiDung);
                }
            }
            return false;
        }
        [HttpPut("RestoreVoucherTaiQuay/{id}")]
        public bool RestoreVoucherTaiQuay(string id)
        {
            var VoucherGet = GetAllVoucher().FirstOrDefault(c => c.IdVoucher == id);
            if (VoucherGet != null)
            {
                if (VoucherGet.NgayBatDau < DateTime.Now && VoucherGet.NgayKetThuc > DateTime.Now)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.HoatDongTaiQuay;
                }
                else if (VoucherGet.NgayBatDau > DateTime.Now && VoucherGet.NgayKetThuc > DateTime.Now)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.ChuaHoatDongTaiQuay;
                }
                else
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.KhongHoatDongTaiQuay;
                }
                return allRepo.EditItem(VoucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucherTaiQuay")]
        public bool UpdateTaiQuay(VoucherDTO voucherDTO)
        {
            var voucherGet = allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == voucherDTO.IdVoucher);

            DateTime NgayTao = voucherGet.NgayTao;
            if (voucherGet != null)
            {
                voucherDTO.MaVoucher = voucherGet.MaVoucher;
                _mapper.Map(voucherDTO, voucherGet);
                if (voucherDTO.TrangThai == null)
                {

                }
                if (voucherGet.NgayBatDau > DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.ChuaHoatDongTaiQuay;
                }
                if (voucherGet.NgayBatDau < DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.HoatDongTaiQuay; ;
                }
                if (voucherGet.SoLuong > 0 && voucherGet.NgayBatDau < DateTime.Now && voucherGet.NgayKetThuc > DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.HoatDongTaiQuay;
                }
                voucherGet.NgayTao = NgayTao;
                return allRepo.EditItem(voucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucherAfterUseItTaiQuay")]
        public bool UpdateVoucherAfterUseItTaiQuay(string idVoucherNguoiDung)
        {
            var voucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCherNguoiDung == idVoucherNguoiDung);
            if (voucherNguoiDung != null)
            {
                voucherNguoiDung.TrangThai = (int)TrangThaiVoucherNguoiDung.DaSuDung;
                VcNguoiDungRepos.EditItem(voucherNguoiDung);
                return true;
            }
            return false;
        }
        [HttpPost("AddVoucherCungBanTaiQuay/{idVoucher}/{idUser}/{soluong}")]
        public bool AddVoucherCungBanTaiQuay(string idVoucher, string idUser, int soluong)
        {
            DateTime? ngay = DateTime.Now;
            var voucher = allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == idVoucher && (c.TrangThai == (int)TrangThaiVoucher.HoatDongTaiQuay || c.TrangThai == (int)TrangThaiVoucher.ChuaHoatDongTaiQuay));


            if (voucher != null)
            {
                //int soLuongDaIn = 0;
                int TrangThaiCanTao = (int)TrangThaiVoucherNguoiDung.KhaDung;

                if (voucher.TrangThai == (int)TrangThaiVoucher.ChuaHoatDongTaiQuay)
                {
                    TrangThaiCanTao = (int)TrangThaiVoucherNguoiDung.ChuaBatDau;
                }

                for (int i = 0; i < soluong; i++)
                {
                    VoucherNguoiDung VcNguoiDungCanThem = new VoucherNguoiDung()
                    {
                        IdVouCherNguoiDung = Guid.NewGuid().ToString(),
                        IdNguoiDung = idUser,
                        IdVouCher = voucher.IdVoucher,
                        NgayNhan = null,
                        TrangThai = TrangThaiCanTao
                    };

                    VcNguoiDungRepos.AddItem(VcNguoiDungCanThem);
                    voucher.SoLuong += 1;
                    allRepo.EditItem(voucher);
                    //soLuongDaIn += 1;

                    // Tạo và lưu hình ảnh QR nếu thêm voucher người dùng thành công
                    string currentDirectory = Directory.GetCurrentDirectory();
                    string rootPath = Directory.GetParent(currentDirectory)!.FullName;
                    string uploadDirectory = Path.Combine(rootPath, "App_View", "wwwroot", "images", "VoucherNguoiDungQRCode");

                    if (!string.IsNullOrEmpty(uploadDirectory) && !string.IsNullOrEmpty(VcNguoiDungCanThem.IdVouCherNguoiDung))
                    {
                        string qrCodeImagePath = Path.Combine(uploadDirectory, VcNguoiDungCanThem.IdVouCherNguoiDung + ".png");

                        if (!Directory.Exists(uploadDirectory))
                        {
                            Directory.CreateDirectory(uploadDirectory);
                        }

                        if (!System.IO.File.Exists(qrCodeImagePath))
                        {
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(VcNguoiDungCanThem.IdVouCherNguoiDung, QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);

                            Bitmap qrCodeImage = qrCode.GetGraphic(20, System.Drawing.Color.DarkBlue, System.Drawing.Color.White, true);

                            using (var stream = new FileStream(qrCodeImagePath, FileMode.Create))
                            {
                                qrCodeImage.Save(stream, ImageFormat.Png);
                            }
                        }
                    }
                }



                return true;
            }

            return false;
        }

        [HttpPut("UpdateTrangThaiKhiXuat")]
        public bool UpdateTrangThaiKhiXuat(List<string> idVoucherNguoiDung)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string rootPath = Directory.GetParent(currentDirectory)!.FullName;
                string uploadDirectory = Path.Combine(rootPath, "App_View", "wwwroot", "images", "VoucherNguoiDungQRCode");

                foreach (var item in idVoucherNguoiDung)
                {
                    var VoucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCherNguoiDung == item);
                    if (VoucherNguoiDung != null)
                    {
                        VoucherNguoiDung.NgayNhan = DateTime.Now;
                        VcNguoiDungRepos.EditItem(VoucherNguoiDung);
                        string oldImagePath = Path.Combine(uploadDirectory, item + ".png");

                        // Kiểm tra và xoá ảnh cũ nếu tồn tại
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            // Thông báo hoặc ghi log trước khi xoá
                            // Xoá ảnh cũ
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }

                // Nếu hàm thực hiện thành công, trả về true
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi xảy ra: " + ex.Message);
                // Ghi log hoặc xử lý ngoại lệ ở đây (nếu cần)
                // Nếu có lỗi, trả về false
                return false;
            }
        }

        #endregion
    }
}
