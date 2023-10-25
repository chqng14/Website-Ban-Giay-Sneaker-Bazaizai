using App_Data.Models;
using App_Data.ViewModels.KhuyenMaiChiTietDTO;

namespace App_View.IServices
{
    public interface IKhuyenMaiServices
    {
        public Task<List<KhuyenMai>> GetAllKhuyenMai();
        public Task<bool> CreateKhuyenMai(KhuyenMai khuyenMai, IFormFile formFile);
    }
}
