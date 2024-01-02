using App_Data.Models;
using App_Data.ViewModels.FilterViewModel;
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
        Task<IEnumerable<SanPhamDanhSachViewModel>> GetAllListViewModelAsync();
        Task<IEnumerable<SanPhamDanhSachViewModel>> GetListSanPhamNgungKinhDoanhViewModelAsync();
        Task<DanhSachGiayViewModel> GetDanhSachGiayViewModelAsync();
        Task<List<SanPhamChiTietDTO>> GetListSanPhamChiTietDTOAsync(List<string> listGuid);
        Task<List<ItemShopViewModel>> GetDanhSachItemShopViewModelAsync();
        Task<List<ItemShopViewModel>> GetDanhSachBienTheItemShopViewModelAsync();
        Task<List<ItemShopViewModel>> GetDanhSachBienTheItemShopViewModelSaleAsync();
        Task<ItemDetailViewModel?> GetItemDetailViewModelAynsc(string id);
        Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectColorAynsc(string id, string mauSac);
        Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectSizeAynsc(string id, int size);
        Task<SanPhamChiTietViewModel?> GetSanPhamChiTietViewModelAynsc(string id);
        Task<bool> NgungKinhDoanhSanPhamAynsc(List<string> lstguid);
        Task<bool> KinhDoanhLaiSanPhamAynsc(List<string> lstguid);
        Task<bool> KhoiPhucKinhDoanhAynsc(string id);
        Task<List<SanPhamChiTietExcelViewModel>> GetListSanPhamExcelAynsc();
        Task<SanPhamChiTietDTO?> GetItemExcelAynsc(BienTheDTO bienTheDTO);
        Task UpdateSoLuongSanPhamChiTietAynsc(string IdSanPhamChiTiet, int soLuong);
        Task<FiltersVM> GetFiltersVMAynsc();
        Task<bool> ProductIsNull(SanPhamChiTietCopyDTO sanPhamChiTietCopyDTO);
        Task UpdateLstSanPhamTableAynsc(List<SanPhamTableDTO> sanPhamTableDTOs);
        List<RelatedProductViewModel> GetRelatedProducts(string sumGuid);
        Task<List<SPDanhSachViewModel>> GetFilteredDaTaDSTongQuanAynsc(ParametersTongQuanDanhSach parametersTongQuanDanhSach);
        IQueryable<SanPhamChiTiet> GetQuerySanPhamChiTiet();
        SanPhamDanhSachViewModel CreateSanPhamDanhSachViewModel(SanPhamChiTiet sanPham);
    }
}
