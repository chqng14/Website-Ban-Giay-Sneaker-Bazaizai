using App_Data.Models;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface ISanPhamChiTietRespo
    {
        Task<IEnumerable<SanPhamChiTiet>> GetListAsync();
        Task<SanPhamChiTiet?> GetByKeyAsync(string id);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(SanPhamChiTiet entity);
        Task<bool> AddAsync(SanPhamChiTiet entity);
        Task<IEnumerable<SanPhamDanhSachViewModel>> GetListViewModelAsync();
        Task<IEnumerable<SanPhamDanhSachViewModel>> GetListSanPhamNgungKinhDoanhViewModelAsync();
        Task<DanhSachGiayViewModel> GetDanhSachGiayViewModelAsync();
        Task<List<SanPhamChiTietDTO>> GetListSanPhamChiTietDTOAsync(List<string> listGuid);
        Task<List<ItemShopViewModel>> GetDanhSachItemShopViewModelAsync();
        Task<ItemDetailViewModel?> GetItemDetailViewModelAynsc(string id);
        Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectColorAynsc(string id, string mauSac);
        Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectSizeAynsc(string id, int size);
        Task<SanPhamChiTietViewModel?> GetSanPhamChiTietViewModelAynsc(string id);

    }
}
