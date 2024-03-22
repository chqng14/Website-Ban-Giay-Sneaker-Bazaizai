using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.Voucher;
using App_View.IServices;
using Newtonsoft.Json;
using OpenXmlPowerTools;
using System.Net.Http;

namespace App_View.Services
{
    public class DanhGiaservice : IDanhGiaservice
    {
        private readonly HttpClient _httpClient;
        public DanhGiaservice()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> CreateDanhGia(DanhGia danhGia)
        {
            try
            {
                string apiUrl = $"https://localhost:7038/api/DanhGia/AddDanhGia?IdDanhGia={danhGia.IdDanhGia}&BinhLuan={danhGia.BinhLuan}&ParentId={danhGia.ParentId}&SaoSp={danhGia.SaoSp}&SaoVanChuyen={danhGia.SaoVanChuyen}&IdNguoiDung={danhGia.IdNguoiDung}&IdSanPhamChiTiet={danhGia.IdSanPhamChiTiet}&MoTa={danhGia.MoTa}&ChatLuongSanPham={danhGia.ChatLuongSanPham}";
                var response = await _httpClient.PostAsync(apiUrl, null);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
        public async Task<bool> CreateDanhGia(string IdDanhGia, string? BinhLuan, string? ParentId,
       int SaoSp, int SaoVanChuyen, string IdNguoiDung, string IdSanPhamChiTiet, string? MoTa, string? ChatLuongSanPham)
        {
            try
            {
                string apiUrl = $"https://localhost:7038/api/DanhGia/AddDanhGia?IdDanhGia={IdDanhGia}&BinhLuan={BinhLuan}&ParentId={ParentId}&SaoSp={SaoSp}&SaoVanChuyen={SaoVanChuyen}&IdNguoiDung={IdNguoiDung}&IdSanPhamChiTiet={IdSanPhamChiTiet}&MoTa={MoTa}&ChatLuongSanPham={ChatLuongSanPham}";
                var response = await _httpClient.PostAsync(apiUrl, null);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }


        public async Task<bool> DeleteDanhGia(string id)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/XoaDanhGia/{id}";
            try
            {
                var response = await _httpClient.DeleteAsync(apiUrl);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }


        public async Task<List<DanhGia>> GetAllDanhGia()
        {
            string apiUrl = "https://localhost:7038/api/DanhGia/GetAllDanhGia";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<DanhGia>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<DanhGia>();
            }
            //string apiUrl = "https://localhost:7038/api/DanhGia/GetAllDanhGia";

            //var response = await _httpClient.GetAsync(apiUrl);
            //string apiData = await response.Content.ReadAsStringAsync();       
            //var DanhGias = JsonConvert.DeserializeObject<List<DanhGia>>(apiData);
            //return DanhGias;
        }

        public async Task<List<DanhGiaViewModel>> GetListAsyncViewModel(string Idchitietsp)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetListAsyncViewModel?idspchitiet={Idchitietsp}";
            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<DanhGiaViewModel>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<DanhGiaViewModel>();
            }
        }
        public async Task<List<DanhGiaViewModel>> GetListAsyncViewModelbyBl(string Idchitietsp)
        {
            try
            {
                var lst = await GetListAsyncViewModel(Idchitietsp);
                var lstWithComments = lst.Where(x => !string.IsNullOrEmpty(x.BinhLuan)).ToList();
                return lstWithComments;



            }
            catch (HttpRequestException)
            {
                return new List<DanhGiaViewModel>();
            }
        }

        public async Task<List<DanhGiaViewModel>> GetListAsyncViewModelWithSao(string Idchitietsp, int SoSao)
        {


            try
            {
                var lst = await GetListAsyncViewModel(Idchitietsp);
                var lstbySao = lst.Where(x => x.SaoSp == SoSao).ToList();
                if (SoSao == 0)
                {
                    return lst;
                }


                return lstbySao;
            }
            catch (HttpRequestException)
            {
                return new List<DanhGiaViewModel>();
            }
        }
        public async Task<DanhGia?> GetDanhGiaById(string id)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetDanhGiaById/{id}";
            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string apiData = await response.Content.ReadAsStringAsync();
                    var danhGia = JsonConvert.DeserializeObject<DanhGia>(apiData);
                    return danhGia;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }


        public async Task<bool> UpdateDanhGia(DanhGia danhGia)
        {
            try
            {
                string apiUrl = $"https://localhost:7038/api/DanhGia/ChinhSuaDanhGia?IdDanhGia={danhGia.IdDanhGia}&BinhLuan={danhGia.BinhLuan}&SaoSp={danhGia.SaoSp}&SaoVanChuyen={danhGia.SaoVanChuyen}&IdNguoiDung={danhGia.IdNguoiDung}&IdSanPhamChiTiet={danhGia.IdSanPhamChiTiet}";

                var content = new StringContent(string.Empty);

                var response = await _httpClient.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }






        public async Task<List<DanhGiaResult>> TongSoDanhGiaCuaMoiSpChuaDuyet()
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetTongSoDanhGiaCuaMoiSpChuaDuyet";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<DanhGiaResult>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<DanhGiaResult>();
            }
        }
        public async Task<List<DanhGiaViewModel>> LstChiTietDanhGiaCuaMoiSpChuaDuyet(string idSanPham)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetLstChiTietDanhGiaCuaMoiSpChuaDuyet?idSanPham={idSanPham}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<DanhGiaViewModel>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<DanhGiaViewModel>();
            }
        }


        public async Task<bool> DuyetDanhGia(string IdDanhGia)
        {
            try
            {
                string apiUrl = $"https://localhost:7038/api/DanhGia/DuyetDanhGia?IdDanhGia={IdDanhGia}";

                var content = new StringContent(string.Empty);

                var response = await _httpClient.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
        public async Task<int> GetTongSoDanhGia(string idspchitiet)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetTongSoDanhGia?idspchitiet={idspchitiet}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<int>(apiData);
            }
            catch (HttpRequestException)
            {
                return 0;
            }
        }
        public async Task<float> GetSoSaoTB(string IdProductChiTiet)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetSoSaoTB?idspchitiet={IdProductChiTiet}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<float>(apiData);
            }
            catch (HttpRequestException)
            {
                return 0;
            }
        }

        public async Task<DanhGiaViewModel?> GetViewModelByKeyAsync(string id)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetDanhGiaViewModelById/{id}";
            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string apiData = await response.Content.ReadAsStringAsync();
                    var danhGia = JsonConvert.DeserializeObject<DanhGiaViewModel>(apiData);
                    return danhGia;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        public async Task<List<DanhGiaViewModel>> GetLstDanhGiaChuaDuyetByDK(int? dk)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetAllDanhGiaChuaDuyetByDkViewModel?Dk={dk}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<DanhGiaViewModel>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<DanhGiaViewModel>();
            }
        }
        public async Task<List<DanhGiaViewModel>> GetLstDanhGiaDaDuyetByDK(int? dk)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetAllDanhGiaDaDuyetByDkViewModel?Dk={dk}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<DanhGiaViewModel>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<DanhGiaViewModel>();
            }
        } 
        public async Task<List<DanhGiaViewModel>> GetAllDanhGiaDaDuyetByNd(string idUser)
        {
            string apiUrl = $"https://localhost:7038/api/DanhGia/GetAllDanhGiaDaDuyetByNd?idUser={idUser}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<DanhGiaViewModel>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<DanhGiaViewModel>();
            }
        }
    }
}
