using App_Data.Models;
using App_Data.ViewModels.VoucherNguoiDung;

namespace App_View.IServices
{
    public interface IVoucherNguoiDungServices
    {
        Task<List<VoucherNguoiDungDTO>> GetAllVouCherNguoiDung();
        Task<List<VoucherNguoiDungDTO>> GetAllVoucherNguoiDungByID(string id);
        Task<VoucherNguoiDung> GetVoucherNguoiDungById(string id);
        Task<bool> AddVoucherNguoiDung(string MaVoucher, string idNguoiDung);
        Task<bool> UpdateVoucherNguoiDungSauKhiDung(VoucherNguoiDungDTO VcDTO);

    }
}
