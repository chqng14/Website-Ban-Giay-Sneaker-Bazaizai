using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;

namespace App_View.IServices
{
    public interface IDanhGiaService
    {
       public Task<List<DanhGia>> GetAllDanhGia();
        public Task<bool> CreateDanhGia(DanhGia DanhGia);
        public Task<bool> UpdateDanhGia(DanhGia DanhGia);
        public Task<bool> DeleteDanhGia(string DanhGia);
    }
}
