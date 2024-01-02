using App_Data.Models;
using App_Data.ViewModels.DanhGia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IDanhGiaRepo
    {
        public Task<List<DanhGia>> GetAllAsync();
        public Task<DanhGia?> GetByKeyAsync(string id);
        public Task<bool> DeleteAsync(string id);
        public Task<List<DanhGiaViewModel>> GetListAsyncViewModel(string IdProductChiTiet);
        public Task<bool> UpdateAsync(DanhGia danhGia);
        public Task<bool> AddAsync(DanhGia danhGia);

        public Task<float> SoSaoTB(string IdProductChiTiet);
        public Task<int> GetTongSoDanhGia(string IdProductChiTiet);
        public Task<DanhGiaViewModel?> GetViewModelByKeyAsync(string id);
        public Task<List<Tuple<string, int,string,string>>> TongSoDanhGiaCuaMoiSpChuaDuyet();
        public Task<List<DanhGiaViewModel>> LstChiTietDanhGiaCuaMoiSpChuaDuyet(string idSanPham);
        public Task<List<DanhGiaViewModel>> LstDanhGia();
    }
}
