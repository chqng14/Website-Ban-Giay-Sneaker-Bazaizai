using App_Data.Models;
using App_Data.ViewModels.VoucherNguoiDung;

namespace App_View.IServices
{
    public interface IVoucherNguoiDungservices
    {
        Task<List<VoucherNguoiDungDTO>> GetAllVouCherNguoiDung();
        Task<List<VoucherNguoiDungDTO>> GetAllVoucherNguoiDungByID(string id);
        Task<VoucherNguoiDung> GetVoucherNguoiDungById(string id);
        Task<bool> AddVoucherNguoiDung(string MaVoucher, string idNguoiDung);
        Task<string> AddVoucherNguoiDungTuAdmin(AddVoucherRequestDTO addVoucherRequestDTO);
        Task<bool> TangVoucherNguoiDungMoi(string ma);
        Task<bool> UpdateVoucherNguoiDungsauKhiDung(VoucherNguoiDungDTO VcDTO);
        Task<VoucherTaiQuayDto> GetVocherTaiQuay(string id);
        bool CheckVoucherInUser(string ma,string idUser);

    }
}
