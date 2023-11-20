using App_Data.Models;
using App_Data.ViewModels.Voucher;

namespace App_View.IServices
{
    public interface IVoucherServices
    {
        Task<List<Voucher>> GetAllVoucher();
        Task<bool> CreateVoucher(VoucherDTO voucherDTO);   
        Task<bool> UpdateVoucher(VoucherDTO voucherDTO);
        Task<bool> DeleteVoucher(string id);
        Task<bool> DeleteVoucherWithList(List<string> Id);
        Task<bool> RestoreVoucherWithList(List<string> Id);
        Task<Voucher> GetVoucherByMa(string ma);
        Task<VoucherDTO> GetVoucherDTOById(string id);
        Task<bool> UpdateVoucherAfterUseIt(string idVoucher);
        Task<bool> UpdateVoucherSoluong(string idVoucher);

        // TaiQuay

        Task<bool> CreateTaiQuay(VoucherDTO voucherDTO);
        Task<bool> DeleteTaiQuay(string id);
        Task<bool> DeleteVoucherWithListTaiQuay(List<string> Id);
        Task<bool> RestoreVoucherWithListTaiQuay(List<string> Id);
        Task<bool> UpdateTaiQuay(VoucherDTO voucherDTO);
        Task<bool> UpdateVoucherAfterUseItTaiQuay(string idVoucher,string idUser);
    }

}
