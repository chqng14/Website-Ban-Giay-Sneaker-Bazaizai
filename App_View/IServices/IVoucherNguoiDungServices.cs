using App_Data.Models;
using App_Data.ViewModels.VoucherNguoiDung;

namespace App_View.IServices
{
    public interface IVoucherNguoiDungServices
    {
        Task<List<VoucherNguoiDung>> GetAllVouCherNguoiDung();
        Task<List<VoucherNguoiDung>> GetAllVoucherNguoiDungByID(string id);
        Task<VoucherNguoiDung> GetVoucherNguoiDungById(string id);
        Task<bool> AddVoucherNguoiDung(VoucherNguoiDungDTO VcDTO);
        Task<bool> UpdateVoucherNguoiDungSauKhiDung(VoucherNguoiDungDTO VcDTO);

    }
}
