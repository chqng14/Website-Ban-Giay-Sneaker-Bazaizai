using App_Data.Models;
using App_Data.ViewModels.Anh;
using App_Data.ViewModels.SanPhamChiTiet;

namespace App_View.IServices
{
    public interface ISanPhamChiTietService
    {
        Task<ResponseCreataDTO> AddAysnc(SanPhamChiTietDTO sanPhamChiTietDTO);
        Task<bool> DeleteAysnc(string id);
        Task<bool> UpdateAynsc(SanPhamChiTietDTO sanPhamChiTietDTO);
        Task<SanPhamChiTiet?> GetByKeyAsync(string id);
        Task<List<SanPhamChiTiet>> GetListSanPhamChiTietAsync();
        Task<List<SanPhamChiTietViewModel>> GetListSanPhamChiTietViewModelAsync();
        Task<ResponseCheckAddOrUpdate> CheckSanPhamAddOrUpdate(SanPhamChiTietDTO sanPhamChiTietDTO);
        Task CreateAnhAysnc(string IdChiTietSp, List<IFormFile> lstIFormFile);
        Task DeleteAnhAysnc(ImageDTO responseImageDeleteVM);
    }
}
