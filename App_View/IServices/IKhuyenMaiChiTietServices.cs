using App_Data.Models;
using App_Data.ViewModels.KhuyenMaiChiTietDTO;

namespace App_View.IServices
{
    public interface IKhuyenMaiChiTietServices
    {
        public Task<List<KhuyenMaiChiTietDTO>> GetAllKhuyenMaiChiTiet();

    }
}
