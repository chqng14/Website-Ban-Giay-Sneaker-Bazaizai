using App_Data.Models;
using App_Data.ViewModels.KhuyenMaiChiTietDTO;

namespace App_View.IServices
{
    public interface IKhuyenMaiChiTietservices
    {
        public Task<List<KhuyenMaiChiTietDTO>> GetAllKhuyenMaiChiTiet();

    }
}
