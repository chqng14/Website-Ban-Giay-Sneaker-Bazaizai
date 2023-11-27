﻿using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.ChatLieuDTO;
using App_Data.ViewModels.KichCoDTO;
using App_Data.ViewModels.KieuDeGiayDTO;
using App_Data.ViewModels.LoaiGiayDTO;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.SanPhamChiTiet.SanPhamDTO;
using App_Data.ViewModels.SanPhamChiTiet.ThuongHieuDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using App_Data.ViewModels.XuatXu;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;
using App_Data.ViewModels.FilterViewModel;
using System.Management.Automation;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamChiTietController : ControllerBase
    {

        private readonly IAllRepo<KichCo> _kickcoRes;
        private readonly IAllRepo<ThuongHieu> _thuongHieuRes;
        private readonly IAllRepo<LoaiGiay> _loaiGiayRes;
        private readonly IAllRepo<KieuDeGiay> _kieuDeGiayRes;
        private readonly IAllRepo<ChatLieu> _chatLieuRes;
        private readonly IAllRepo<SanPham> _sanPhamRes;
        private readonly IAllRepo<MauSac> _mauSacRes;
        private readonly IAllRepo<XuatXu> _xuatXuRes;
        private readonly ISanPhamChiTietRespo _sanPhamChiTietRes;
        private readonly IAllRepo<Anh> _AnhRes;
        private readonly IMapper _mapper;
        private readonly AnhController _anhController;

        public SanPhamChiTietController(IAllRepo<KichCo> kickcoRes, IAllRepo<ThuongHieu> thuongHieuRes, IAllRepo<LoaiGiay> loaiGiayRes, IAllRepo<KieuDeGiay> kieuDeGiayRes, IAllRepo<ChatLieu> chatLieuRes, IAllRepo<SanPham> sanPhamRes, IAllRepo<XuatXu> xuatXuRes, IAllRepo<MauSac> mauSacRes, ISanPhamChiTietRespo sanPhamChiTietRes, IMapper mapper, IAllRepo<Anh> anhRes, AnhController anhController)
        {
            _kickcoRes = kickcoRes;
            _thuongHieuRes = thuongHieuRes;
            _loaiGiayRes = loaiGiayRes;
            _kieuDeGiayRes = kieuDeGiayRes;
            _chatLieuRes = chatLieuRes;
            _sanPhamRes = sanPhamRes;
            _xuatXuRes = xuatXuRes;
            _mauSacRes = mauSacRes;
            _sanPhamChiTietRes = sanPhamChiTietRes;
            _mapper = mapper;
            _AnhRes = anhRes;
            _anhController = anhController;
        }

        [HttpPost("check-add-or-update")]
        public async Task<ResponseCheckAddOrUpdate> CheckProductForAddOrUpdate(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            var productDetails = (await _sanPhamChiTietRes.GetListAsync())
                .FirstOrDefault(x =>
                x.IdXuatXu == sanPhamChiTietDTO.IdXuatXu &&
                x.IdMauSac == sanPhamChiTietDTO.IdMauSac &&
                x.IdKichCo == sanPhamChiTietDTO.IdKichCo &&
                x.IdChatLieu == sanPhamChiTietDTO.IdChatLieu &&
                x.IdSanPham == sanPhamChiTietDTO.IdSanPham &&
                x.IdThuongHieu == sanPhamChiTietDTO.IdThuongHieu &&
                x.IdLoaiGiay == sanPhamChiTietDTO.IdLoaiGiay &&
                x.IdKieuDeGiay == sanPhamChiTietDTO.IdKieuDeGiay
                );

            if (productDetails != null)
            {
                var productDetaiDTOMap = _mapper.Map<SanPhamChiTietDTO>(productDetails);
                productDetaiDTOMap.DanhSachAnh = _AnhRes.GetAll()
                                .Where(img => img.TrangThai == 0 && img.IdSanPhamChiTiet == productDetaiDTOMap.IdChiTietSp)
                                .Select(x => x.Url)
                                .ToList()!;
                return new ResponseCheckAddOrUpdate() { Success = true, Data = productDetaiDTOMap };

            }
            return new ResponseCheckAddOrUpdate() { Success = false, Data = null };
        }

        [HttpGet("Get-List-SanPhamChiTietViewModel")]
        public async Task<List<SanPhamDanhSachViewModel>> GetListSanPham()
        {
            return (await _sanPhamChiTietRes.GetListViewModelAsync()).ToList();
        }

        [HttpGet("Get-List-SanPhamChiTiet")]
        public async Task<List<SanPhamChiTiet>> GetListSanPhamChiTiet()
        {
            return (await _sanPhamChiTietRes.GetListAsync()).ToList();
        }

        [HttpPost("Get-List-SanPhamChiTietDTO")]
        public async Task<List<SanPhamChiTietDTO>> GetListSanPhamChiTietDTO(List<string> lstGuid)
        {
            return await _sanPhamChiTietRes.GetListSanPhamChiTietDTOAsync(lstGuid);
        }

        [HttpGet("Get-DanhSachGiayViewModel")]
        public async Task<DanhSachGiayViewModel> GetDanhSachGiay()
        {
            return await _sanPhamChiTietRes.GetDanhSachGiayViewModelAsync(); ;
        }

        [HttpGet("khoi-phuc-kinh-doanh/{id}")]
        public async Task<bool> KhoiPhucKinhDoanh(string id)
        {
            return await _sanPhamChiTietRes.KhoiPhucKinhDoanhAynsc(id);
        }

        [HttpGet("Get-List-SanPhamNgungKinhDoanhViewModel")]
        public async Task<List<SanPhamDanhSachViewModel>> GetDanhSachGiayNgungKinhDoanh()
        {
            return (await _sanPhamChiTietRes.GetListSanPhamNgungKinhDoanhViewModelAsync()).ToList();
        }

        [HttpPut("UpdateSoLuong")]
        public async Task UpDateSoLuong(SanPhamSoLuongDTO sanPhamSoLuongDTO)
        {
            await _sanPhamChiTietRes.UpdateSoLuongSanPhamChiTietAynsc(sanPhamSoLuongDTO.IdChiTietSanPham, sanPhamSoLuongDTO.SoLuong);
        }

        [HttpGet("Get-List-ItemShopViewModel")]
        public async Task<List<ItemShopViewModel>?> GetDanhSachItemShowViewModel()
        {
            return await _sanPhamChiTietRes.GetDanhSachItemShopViewModelAsync();
        }

        [HttpGet("Get-List-ItemBienTheShopViewModel")]
        public async Task<List<ItemShopViewModel>?> GetDanhSachItemBienTheShowViewModel()
        {
            return await _sanPhamChiTietRes.GetDanhSachBienTheItemShopViewModelAsync();
        }

        [HttpGet("Get-List-ItemBienTheShopViewModelSale")]
        public async Task<List<ItemShopViewModel>?> GetDanhSachItemBienTheShowViewModelSale()
        {
            return await _sanPhamChiTietRes.GetDanhSachBienTheItemShopViewModelSaleAsync();
        }

        [HttpGet("Get-SanPhamChiTiet/{id}")]
        public async Task<SanPhamChiTiet?> GetSanPham(string id)
        {
            return await _sanPhamChiTietRes.GetByKeyAsync(id);
        }

        [HttpGet("Get-SanPhamChiTietViewModel/{id}")]
        public async Task<SanPhamChiTietViewModel?> GetSanPhamViewModel(string id)
        {
            return await _sanPhamChiTietRes.GetSanPhamChiTietViewModelAynsc(id);
        }

        [HttpGet("Get-ItemDetailViewModel/{id}")]
        public async Task<ItemDetailViewModel?> GetItemDetailViewModel(string id)
        {
            return await _sanPhamChiTietRes.GetItemDetailViewModelAynsc(id);
        }

        [HttpGet("Get-ItemDetailViewModel/{id}/{mauSac}")]
        public async Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectColor(string id, string mauSac)
        {
            return await _sanPhamChiTietRes.GetItemDetailViewModelWhenSelectColorAynsc(id, mauSac);
        }

        [HttpGet("Get-ItemDetailViewModel/idsanpham/{id}/size/{size}")]
        public async Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectSize(string id, int size)
        {
            return await _sanPhamChiTietRes.GetItemDetailViewModelWhenSelectSizeAynsc(id, size);
        }

        [HttpPost("Creat-SanPhamChiTiet")]
        public async Task<ResponseCreateDTO> CreateSanPhamChiTiet(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            try
            {
                var sanPhamChiTietCheck = (await _sanPhamChiTietRes.GetListAsync())
                .FirstOrDefault(x =>
                x.IdSanPham == sanPhamChiTietDTO!.IdSanPham &&
                x.IdChatLieu == sanPhamChiTietDTO.IdChatLieu &&
                x.IdKichCo == sanPhamChiTietDTO.IdKichCo &&
                x.IdMauSac == sanPhamChiTietDTO.IdMauSac &&
                x.IdKieuDeGiay == sanPhamChiTietDTO.IdKieuDeGiay &&
                x.IdLoaiGiay == sanPhamChiTietDTO.IdLoaiGiay &&
                x.IdThuongHieu == sanPhamChiTietDTO.IdThuongHieu &&
                x.IdXuatXu == sanPhamChiTietDTO.IdXuatXu
                );
                if (sanPhamChiTietCheck == null)
                {
                    string currentDirectory = Directory.GetCurrentDirectory();
                    string rootPath = Directory.GetParent(currentDirectory)!.FullName;
                    string uploadDirectory = Path.Combine(rootPath, "App_View", "wwwroot", "images", "QRCode");

                    var sanPhamChiTiet = _mapper.Map<SanPhamChiTiet>(sanPhamChiTietDTO);
                    sanPhamChiTiet.IdChiTietSp = Guid.NewGuid().ToString();
                    var mauSac = _mauSacRes.GetAll().FirstOrDefault(ms => ms.IdMauSac == sanPhamChiTietDTO.IdMauSac)!.TenMauSac!.Substring(0, 2);
                    var size = _kickcoRes.GetAll().FirstOrDefault(kc => kc.IdKichCo == sanPhamChiTietDTO.IdKichCo)!.SoKichCo;
                    sanPhamChiTiet.Ma = "SP-" + ((int)1000 + (await _sanPhamChiTietRes.GetListAsync()).Count()).ToString() + "-" + mauSac + "-" + size;
                    sanPhamChiTiet.TrangThai = 0;
                    sanPhamChiTiet.SoLuongDaBan = 0;
                    sanPhamChiTiet.NgayTao = DateTime.Now;

                    if (!string.IsNullOrEmpty(uploadDirectory) && !string.IsNullOrEmpty(sanPhamChiTiet.Ma))
                    {
                        string qrCodeImagePath = Path.Combine(uploadDirectory, sanPhamChiTiet.Ma + ".png");

                        if (!Directory.Exists(uploadDirectory))
                        {
                            Directory.CreateDirectory(uploadDirectory);
                        }

                        if (!System.IO.File.Exists(qrCodeImagePath))
                        {
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(sanPhamChiTiet.Ma, QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);

                            Bitmap qrCodeImage = qrCode.GetGraphic(20, System.Drawing.Color.DarkBlue, System.Drawing.Color.White, true);

                            using (var stream = new FileStream(qrCodeImagePath, FileMode.Create))
                            {
                                qrCodeImage.Save(stream, ImageFormat.Png);
                            }
                        }
                    }

                    return new ResponseCreateDTO()
                    {
                        Success = await _sanPhamChiTietRes.AddAsync(sanPhamChiTiet),
                        IdChiTietSp = sanPhamChiTiet.IdChiTietSp
                    };
                }
                return new ResponseCreateDTO()
                {
                    Success = false,
                    DescriptionErr = "Sản phẩm đã tồn tại"
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResponseCreateDTO()
                {
                    Success = false,
                    DescriptionErr = ex.Message
                };
            }
        }

        [HttpPost("Creat-SanPhamChiTietCopy")]
        public async Task<bool> CreateSanPhamChiTietCoppy([FromForm] SanPhamChiTietCopyDTO sanPhamChiTietCopyDTO)
        {
            try
            {
                if ((await _sanPhamChiTietRes.ProductIsNull(sanPhamChiTietCopyDTO)))
                {
                    string currentDirectory = Directory.GetCurrentDirectory();
                    string rootPath = Directory.GetParent(currentDirectory)!.FullName;
                    string uploadDirectory = Path.Combine(rootPath, "App_View", "wwwroot", "images", "QRCode");

                    var sanPhamChiTiet = _mapper.Map<SanPhamChiTiet>(sanPhamChiTietCopyDTO.SanPhamChiTietData);
                    sanPhamChiTiet.IdChiTietSp = Guid.NewGuid().ToString();
                    var mauSac = _mauSacRes.GetAll().FirstOrDefault(ms => ms.IdMauSac == sanPhamChiTiet.IdMauSac)!.TenMauSac!.Substring(0, 2);
                    var size = _kickcoRes.GetAll().FirstOrDefault(kc => kc.IdKichCo == sanPhamChiTiet.IdKichCo)!.SoKichCo;
                    sanPhamChiTiet.Ma = "SP-" + ((int)1000 + (await _sanPhamChiTietRes.GetListAsync()).Count()).ToString() + "-" + mauSac + "-" + size;
                    sanPhamChiTiet.TrangThai = 0;
                    sanPhamChiTiet.SoLuongDaBan = 0;
                    sanPhamChiTiet.NgayTao = DateTime.Now;

                    QRCodeGenerator qrGenerator = new QRCodeGenerator();

                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(sanPhamChiTiet.Ma, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);

                    Bitmap qrCodeImage = qrCode.GetGraphic(20, System.Drawing.Color.DarkBlue, System.Drawing.Color.White, true);

                    string qrCodeImagePath = Path.Combine(uploadDirectory, sanPhamChiTiet.Ma + ".png");

                    using (var stream = new FileStream(qrCodeImagePath, FileMode.Create))
                    {
                        qrCodeImage.Save(stream, ImageFormat.Png);
                    }

                    await _sanPhamChiTietRes.AddAsync(sanPhamChiTiet);

                    if (sanPhamChiTietCopyDTO.SanPhamChiTietData!.DanhSachAnh != null)
                    {
                        if (sanPhamChiTietCopyDTO.ListTenAnhRemove == null) sanPhamChiTietCopyDTO.ListTenAnhRemove = new List<string>();
                        var listAnhCopy = sanPhamChiTietCopyDTO.SanPhamChiTietData!.DanhSachAnh!.Except(sanPhamChiTietCopyDTO.ListTenAnhRemove!).ToList();
                        listAnhCopy.ForEach(tenAnh =>
                        {
                            _AnhRes.AddItem(new Anh
                            {
                                IdAnh = Guid.NewGuid().ToString(),
                                IdSanPhamChiTiet = sanPhamChiTiet.IdChiTietSp,
                                TrangThai = 0,
                                NgayTao = DateTime.Now,
                                Url = tenAnh
                            });
                        });
                    }

                    if (sanPhamChiTietCopyDTO.ListAnhCreate != null && sanPhamChiTietCopyDTO.ListAnhCreate!.Any())
                    {
                        await _anhController.CreateImage(sanPhamChiTiet.IdChiTietSp, sanPhamChiTietCopyDTO.ListAnhCreate!);
                    }
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        [HttpPut("Ngung_Kinh_Doanh_List_SanPham")]
        public async Task<bool> NgungKinhDoanhSanPham(List<string> lstGuild)
        {
            return await _sanPhamChiTietRes.NgungKinhDoanhSanPhamAynsc(lstGuild);
        }

        [HttpPut("Update-Kinh_Doanh_List_SanPham")]
        public async Task<bool> KinhDoanhLaiSanPham(List<string> lstGuild)
        {
            return await _sanPhamChiTietRes.KinhDoanhLaiSanPhamAynsc(lstGuild);
        }

        [HttpDelete("Delete-SanPhamChiTiet/{id}")]
        public async Task<bool> DeleteSanPham(string id)
        {
            var sanPhamChiTiet = await GetSanPham(id);
            if (sanPhamChiTiet != null)
            {
                sanPhamChiTiet.TrangThai = 1;
                return await _sanPhamChiTietRes.UpdateAsync(sanPhamChiTiet);
            }
            return false;
        }

        [HttpPut("Update-SanPhamChiTiet")]
        public async Task<bool> Update(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            var spChiTiet = await GetSanPham(sanPhamChiTietDTO.IdChiTietSp!);

            if (spChiTiet != null)
            {
                var sanPhamChiTietCheck = (await _sanPhamChiTietRes.GetListAsync())
                .FirstOrDefault(x =>
                x.IdSanPham == sanPhamChiTietDTO!.IdSanPham &&
                x.IdChatLieu == sanPhamChiTietDTO.IdChatLieu &&
                x.IdKichCo == sanPhamChiTietDTO.IdKichCo &&
                x.IdMauSac == sanPhamChiTietDTO.IdMauSac &&
                x.IdKieuDeGiay == sanPhamChiTietDTO.IdKieuDeGiay &&
                x.IdLoaiGiay == sanPhamChiTietDTO.IdLoaiGiay &&
                x.IdThuongHieu == sanPhamChiTietDTO.IdThuongHieu &&
                x.IdXuatXu == sanPhamChiTietDTO.IdXuatXu
                );
                if (
                (sanPhamChiTietDTO.IdChatLieu == spChiTiet.IdChatLieu &&
                sanPhamChiTietDTO.IdKieuDeGiay == spChiTiet.IdKieuDeGiay &&
                sanPhamChiTietDTO.IdXuatXu == spChiTiet.IdXuatXu &&
                sanPhamChiTietDTO.IdLoaiGiay == spChiTiet.IdLoaiGiay &&
                sanPhamChiTietDTO.IdThuongHieu == spChiTiet.IdThuongHieu &&
                sanPhamChiTietDTO.IdSanPham == spChiTiet.IdSanPham &&
                sanPhamChiTietDTO.IdMauSac == spChiTiet.IdMauSac &&
                sanPhamChiTietDTO.IdKichCo == spChiTiet.IdKichCo) || sanPhamChiTietCheck == null
                )
                {
                    _mapper.Map(sanPhamChiTietDTO, spChiTiet);
                    return await _sanPhamChiTietRes.UpdateAsync(spChiTiet);
                }
            }
            return false;
        }

        [HttpGet("get_list_SanPhamExcel")]
        public async Task<List<SanPhamChiTietExcelViewModel>> GetLstSanPhamExcel()
        {
            return await _sanPhamChiTietRes.GetListSanPhamExcelAynsc();
        }

        [HttpPost("get-ItemExcel")]
        public async Task<SanPhamChiTietDTO> GetItemExcel(BienTheDTO bienTheDTO)
        {
            return await _sanPhamChiTietRes.GetItemExcelAynsc(bienTheDTO);
        }

        //GetItemfilterVm
        [HttpGet("get-ItemFilterVM")]
        public async Task<FiltersVM> GetItemFilterVM()
        {
            return await _sanPhamChiTietRes.GetFiltersVMAynsc();
        }

        #region AddVariants
        //SanPham
        [HttpPost("Create-SanPham")]
        public SanPhamDTO? CreateSanPham(SanPhamDTO sanPhamDTO)
        {

            if (!_sanPhamRes.GetAll().Where(sp => sp.Trangthai == 0).Select(i => i.TenSanPham).Contains(sanPhamDTO.TenSanPham))
            {
                var sanPham = _mapper.Map<SanPham>(sanPhamDTO);
                sanPham.IdSanPham = Guid.NewGuid().ToString();
                sanPham.MaSanPham = !_sanPhamRes.GetAll().Any() ? "SP1" : "SP" + _sanPhamRes.GetAll().Count() + 1;
                sanPham.Trangthai = 0;
                _sanPhamRes.AddItem(sanPham);
                sanPhamDTO.IdSanPham = sanPham.IdSanPham;
                return sanPhamDTO;
            }
            return null;
        }

        //ThuongHieu
        [HttpPost("Create-ThuongHieu")]
        public ThuongHieuDTO? CreateThuongHieu(ThuongHieuDTO thuongHieuDTO)
        {
            if (!_thuongHieuRes.GetAll().Where(sp => sp.TrangThai == 0).Select(i => i.TenThuongHieu).Contains(thuongHieuDTO.TenThuongHieu))
            {
                var thuongHieu = _mapper.Map<ThuongHieu>(thuongHieuDTO);
                thuongHieu.IdThuongHieu = Guid.NewGuid().ToString();
                thuongHieu.MaThuongHieu = !_xuatXuRes.GetAll().Any() ? "TH1" : "TH" + _xuatXuRes.GetAll().Count() + 1;
                thuongHieu.TrangThai = 0;
                _thuongHieuRes.AddItem(thuongHieu);
                thuongHieuDTO.IdThuongHieu = thuongHieu.IdThuongHieu;
                return thuongHieuDTO;
            }
            return null;
        }

        //XuatXu
        [HttpPost("Create-XuatXu")]
        public XuatXuDTO? CreateXuatXu(XuatXuDTO xuaXuDTO)
        {
            if (!_xuatXuRes.GetAll().Where(sp => sp.TrangThai == 0).Select(i => i.Ten).Contains(xuaXuDTO.Ten))
            {
                var xuatXu = _mapper.Map<XuatXu>(xuaXuDTO);
                xuatXu.IdXuatXu = Guid.NewGuid().ToString();
                xuatXu.Ma = !_xuatXuRes.GetAll().Any() ? "XX1" : "XX" + _xuatXuRes.GetAll().Count() + 1;
                xuatXu.TrangThai = 0;
                _xuatXuRes.AddItem(xuatXu);
                xuaXuDTO.IdXuatXu = xuatXu.IdXuatXu;
                return xuaXuDTO;
            }
            return null;
        }

        //ChatLieu
        [HttpPost("Create-ChatLieu")]
        public ChatLieuDTO? CreateChatLieu(ChatLieuDTO chatLieuDTO)
        {
            if (!_chatLieuRes.GetAll().Where(sp => sp.TrangThai == 0).Select(i => i.TenChatLieu).Contains(chatLieuDTO.TenChatLieu))
            {
                var chatLieu = _mapper.Map<ChatLieu>(chatLieuDTO);
                chatLieu.IdChatLieu = Guid.NewGuid().ToString();
                chatLieu.MaChatLieu = !_chatLieuRes.GetAll().Any() ? "CL1" : "CL" + _chatLieuRes.GetAll().Count() + 1;
                chatLieu.TrangThai = 0;
                _chatLieuRes.AddItem(chatLieu);
                chatLieuDTO.IdChatLieu = chatLieu.IdChatLieu;
                return chatLieuDTO;
            }
            return null;
        }

        //LoaiGiay
        [HttpPost("Create-LoaiGiay")]
        public LoaiGiayDTO? CreateLoaiGiay(LoaiGiayDTO loaiGiayDTO)
        {
            if (!_loaiGiayRes.GetAll().Where(sp => sp.TrangThai == 0).Select(i => i.TenLoaiGiay).Contains(loaiGiayDTO.TenLoaiGiay))
            {
                var loaiGiay = _mapper.Map<LoaiGiay>(loaiGiayDTO);
                loaiGiay.IdLoaiGiay = Guid.NewGuid().ToString();
                loaiGiay.MaLoaiGiay = !_loaiGiayRes.GetAll().Any() ? "LG1" : "LG" + _loaiGiayRes.GetAll().Count() + 1;
                loaiGiay.TrangThai = 0;
                _loaiGiayRes.AddItem(loaiGiay);
                loaiGiayDTO.IdLoaiGiay = loaiGiay.IdLoaiGiay;
                return loaiGiayDTO;
            }
            return null;
        }

        //KieuDeGiay
        [HttpPost("Create-KieuDeGiay")]
        public KieuDeGiayDTO? CreateKieuDeGiay(KieuDeGiayDTO kieuDeGiay)
        {
            if (!_kieuDeGiayRes.GetAll().Where(sp => sp.Trangthai == 0).Select(i => i.TenKieuDeGiay).Contains(kieuDeGiay.TenKieuDeGiay))
            {
                var loaiGiay = _mapper.Map<KieuDeGiay>(kieuDeGiay);
                loaiGiay.IdKieuDeGiay = Guid.NewGuid().ToString();
                loaiGiay.MaKieuDeGiay = !_kieuDeGiayRes.GetAll().Any() ? "KDG1" : "KDG" + _kieuDeGiayRes.GetAll().Count() + 1;
                loaiGiay.Trangthai = 0;
                _kieuDeGiayRes.AddItem(loaiGiay);
                kieuDeGiay.IdKieuDeGiay = loaiGiay.IdKieuDeGiay;
                return kieuDeGiay;
            }
            return null;
        }

        //MauSac
        [HttpPost("Create-MauSac")]
        public MauSacDTO? CreateMauSac(MauSacDTO kieuDeGiay)
        {
            if (!_mauSacRes.GetAll().Where(sp => sp.TrangThai == 0).Select(i => i.TenMauSac).Contains(kieuDeGiay.TenMauSac))
            {
                var loaiGiay = _mapper.Map<MauSac>(kieuDeGiay);
                loaiGiay.IdMauSac = Guid.NewGuid().ToString();
                loaiGiay.MaMauSac = !_mauSacRes.GetAll().Any() ? "MS1" : "MS" + _mauSacRes.GetAll().Count() + 1;
                loaiGiay.TrangThai = 0;
                _mauSacRes.AddItem(loaiGiay);
                kieuDeGiay.IdMauSac = loaiGiay.IdMauSac;
                return kieuDeGiay;
            }
            return null;
        }

        //KichCo
        [HttpPost("Create-KichCo")]
        public KichCoDTO? CreateKichCo(KichCoDTO kieuDeGiay)
        {
            if (!_kickcoRes.GetAll().Where(sp => sp.TrangThai == 0).Select(i => i.SoKichCo).Contains(kieuDeGiay.SoKichCo))
            {
                var loaiGiay = _mapper.Map<KichCo>(kieuDeGiay);
                loaiGiay.IdKichCo = Guid.NewGuid().ToString();
                loaiGiay.MaKichCo = !_kickcoRes.GetAll().Any() ? "MS1" : "MS" + _kickcoRes.GetAll().Count() + 1;
                loaiGiay.TrangThai = 0;
                _kickcoRes.AddItem(loaiGiay);
                kieuDeGiay.IdKichCo = loaiGiay.IdKichCo;
                return kieuDeGiay;
            }
            return null;
        }
        #endregion

        #region GetListVariants
        [HttpGet("Get-List-ChatLieu")]
        public List<ChatLieu>? GetListChatLieu()
        {
            return _chatLieuRes.GetAll().ToList();
        }

        [HttpGet("Get-List-KichCo")]
        public List<KichCo>? GetListModelKichCo()
        {
            return _kickcoRes.GetAll().ToList();
        }

        [HttpGet("Get-List-KieuDeGiay")]
        public List<KieuDeGiay>? GetListModelKieuDeGiay()
        {
            return _kieuDeGiayRes.GetAll().ToList();
        }

        [HttpGet("Get-List-LoaiGiay")]
        public List<LoaiGiay>? GetListModelLoaiGiay()
        {
            return _loaiGiayRes.GetAll().ToList();
        }

        [HttpGet("Get-List-MauSac")]
        public List<MauSac>? GetListModelMauSac()
        {
            return _mauSacRes.GetAll().ToList();
        }

        [HttpGet("Get-List-SanPham")]
        public List<SanPham>? GetListModelSanPham()
        {
            return _sanPhamRes.GetAll().ToList();
        }

        [HttpGet("Get-List-ThuongHieu")]
        public List<ThuongHieu>? GetListModelThuongHieu()
        {
            return _thuongHieuRes.GetAll().ToList();
        }

        [HttpGet("Get-List-XuatXu")]
        public List<XuatXu>? GetListModelXuatXu()
        {
            return _xuatXuRes.GetAll().ToList();
        }
        #endregion

        [HttpPut("Update-List-SanPhamTable")]
        public async Task<bool> UpdateListSanPhamTableDTO(List<SanPhamTableDTO> lstSanPhamTableDTO)
        {
            try
            {
                await _sanPhamChiTietRes.UpdateLstSanPhamTableAynsc(lstSanPhamTableDTO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet("Get-List-RelatedProduct")]
        public List<RelatedProductViewModel> GetRelatedProducts(string sumGuild)
        {
            return _sanPhamChiTietRes.GetRelatedProducts(sumGuild);
        }

        [HttpPost("LayDanhSachTongQuan")]
        public async Task<IActionResult> LayDanhSachDienTu([FromBody] ParametersTongQuanDanhSach parameters)
        {
            try
            {
                var viewModelResult = await _sanPhamChiTietRes.GetFilteredDaTaDSTongQuanAynsc(parameters);

                var paginatedResult = viewModelResult
                    .Skip(parameters.Start)
                    .Take(parameters.Length)
                    .ToList();

                return new ObjectResult(new
                {
                    draw = parameters.Draw,
                    recordsTotal = viewModelResult.Count,
                    recordsFiltered = viewModelResult.Count,
                    data = paginatedResult
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
