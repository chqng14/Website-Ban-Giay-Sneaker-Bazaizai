using App_View.IServices;
using App_Data.Models;
using System.Net.Http.Json;
using App_Data.ViewModels.Anh;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using App_Data.ViewModels.SanPhamChiTiet.SanPhamDTO;
using App_Data.ViewModels.SanPhamChiTiet.ThuongHieuDTO;
using App_Data.ViewModels.XuatXu;
using App_Data.ViewModels.ChatLieuDTO;
using App_Data.ViewModels.LoaiGiayDTO;
using App_Data.ViewModels.KieuDeGiayDTO;
using App_Data.ViewModels.KichCoDTO;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using App_Data.ViewModels.SanPhamChiTietDTO;
using static App_View.Areas.Admin.Controllers.SanPhamChiTietController;

namespace App_View.Services
{
    public class SanPhamChiTietservice : ISanPhamChiTietservice
    {
        private readonly HttpClient _httpClient;

        public SanPhamChiTietservice(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseCreateDTO> AddAysnc(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Creat-SanPhamChiTiet", sanPhamChiTietDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ResponseCreateDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<ResponseCheckAddOrUpdate> CheckSanPhamAddOrUpdate(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/check-add-or-update", sanPhamChiTietDTO);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ResponseCheckAddOrUpdate>();
                }
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Not IsSuccessStatusCode");
            }


        }

        public async Task CreateAnhAysnc(string IdChiTietSp, List<IFormFile> lstIFormFile)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                content.Add(new StringContent(IdChiTietSp.ToString()), "idProductDetail");

                foreach (var file in lstIFormFile)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    content.Add(streamContent, "lstIFormFile", file.FileName);
                }

                var response = await _httpClient.PostAsync("/api/Anh/create-list-image", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Delete successful. Response: " + responseContent);
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Delete failed. Response: " + responseContent);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace); ;
            }
        }

        public async Task<ChatLieuDTO?> CreateTenChatLieuAynsc(ChatLieuDTO xuatXuDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Create-ChatLieu", xuatXuDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ChatLieuDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<KichCoDTO?> CreateTenKichCoAynsc(KichCoDTO kichCoDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Create-KichCo", kichCoDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<KichCoDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<KieuDeGiayDTO?> CreateTenKieuDeGiayAynsc(KieuDeGiayDTO kieuDeGiayDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Create-KieuDeGiay", kieuDeGiayDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<KieuDeGiayDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<LoaiGiayDTO?> CreateTenLoaiGiayAynsc(LoaiGiayDTO loaiGiayDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Create-LoaiGiay", loaiGiayDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<LoaiGiayDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<MauSacDTO?> CreateTenMauSacAynsc(MauSacDTO mauSacDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Create-MauSac", mauSacDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<MauSacDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<SanPhamDTO?> CreateTenSanPhamAynsc(SanPhamDTO sanPhamDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Create-SanPham", sanPhamDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<SanPhamDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<ThuongHieuDTO?> CreateTenThuongHieuAynsc(ThuongHieuDTO thuongHieu)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Create-ThuongHieu", thuongHieu);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ThuongHieuDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<XuatXuDTO?> CreateTenXuatXuAynsc(XuatXuDTO xuatXuDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Create-XuatXu", xuatXuDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<XuatXuDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task DeleteAnhAysnc(ImageDTO responseImageDeleteVM)
        {
            try
            {
                var deleteUrl = "/api/Anh/delete-list-image";
                var content = new StringContent(JsonConvert.SerializeObject(responseImageDeleteVM), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Delete, deleteUrl);
                request.Content = content;

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<bool> DeleteAysnc(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/SanPhamChiTiet/Delete-SanPhamChiTiet/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                throw new Exception("Not IsSuccessStatusCode");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<SanPhamChiTiet?> GetByKeyAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<SanPhamChiTiet?>($"/api/SanPhamChiTiet/Get-SanPhamChiTiet/{id}");
        }

        public async Task<List<SanPhamDanhSachViewModel>> GetDanhSachGiayNgungKinhDoanhAynsc()
        {
            return (await _httpClient.GetFromJsonAsync<List<SanPhamDanhSachViewModel>>("/api/SanPhamChiTiet/Get-List-SanPhamNgungKinhDoanhViewModel"))!;
        }

        public Task<DanhSachGiayViewModel?> GetDanhSachGiayViewModelAynsc()
        {
            return _httpClient.GetFromJsonAsync<DanhSachGiayViewModel?>("/api/SanPhamChiTiet/Get-DanhSachGiayViewModel");
        }

        public async Task<ItemDetailViewModel?> GetItemDetailViewModelAynsc(string id)
        {
            return await _httpClient.GetFromJsonAsync<ItemDetailViewModel?>($"/api/SanPhamChiTiet/Get-ItemDetailViewModel/{id}");
        }

        public Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectColorAynsc(string id, string mauSac)
        {
            return _httpClient.GetFromJsonAsync<ItemDetailViewModel?>($"/api/SanPhamChiTiet/Get-ItemDetailViewModel/{id}/{mauSac}");
        }

        public Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectSizeAynsc(string id, int size)
        {
            return _httpClient.GetFromJsonAsync<ItemDetailViewModel?>($"/api/SanPhamChiTiet/Get-ItemDetailViewModel/idsanpham/{id}/size/{size}");
        }

        public async Task<List<ItemShopViewModel>?> GetListItemShopViewModelAynsc()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ItemShopViewModel>?>("/api/SanPhamChiTiet/Get-List-ItemShopViewModel");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<ItemShopViewModel>();
            }
        }

        public async Task<List<SanPhamChiTiet>> GetListSanPhamChiTietAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<SanPhamChiTiet>>("/api/SanPhamChiTiet/Get-List-SanPhamChiTiet"))!;
        }

        public async Task<List<SanPhamChiTietDTO>> GetListSanPhamChiTietDTOAsync(ListGuildDTO listGuildDTO)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(listGuildDTO.listGuild), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/SanPhamChiTiet/Get-List-SanPhamChiTietDTO", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<SanPhamChiTietDTO>>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<List<SanPhamDanhSachViewModel>> GetListSanPhamChiTietViewModelAsync()
        {
            try
            {
                return (await _httpClient.GetFromJsonAsync<List<SanPhamDanhSachViewModel>>("/api/SanPhamChiTiet/Get-List-SanPhamChiTietViewModel"))!;
            }
            catch (Exception)
            {
                return new List<SanPhamDanhSachViewModel>();
            }
        }
        public async Task<List<SanPhamDanhSachViewModel>> GetAllListSanPhamChiTietViewModelAsync()
        {
            try
            {
                return (await _httpClient.GetFromJsonAsync<List<SanPhamDanhSachViewModel>>("/api/SanPhamChiTiet/GetAll-List-SanPhamChiTietViewModel"))!;
            }
            catch (Exception)
            {
                return new List<SanPhamDanhSachViewModel>();
            }
        }
        public async Task<SanPhamChiTietViewModel?> GetSanPhamChiTietViewModelByKeyAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<SanPhamChiTietViewModel?>($"/api/SanPhamChiTiet/Get-SanPhamChiTietViewModel/{id}");
        }

        public Task<bool> KhoiPhucKinhDoanhAynsc(string id)
        {
            return _httpClient.GetFromJsonAsync<bool>($"/api/SanPhamChiTiet/khoi-phuc-kinh-doanh/{id}");
        }

        public async Task<bool> KinhDoanhLaiSanPhamAynsc(ListGuildDTO lstGuid)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(lstGuid.listGuild), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("/api/SanPhamChiTiet/Update-Kinh_Doanh_List_SanPham", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<bool> NgungKinhDoanhSanPhamAynsc(ListGuildDTO lstGuid)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(lstGuid.listGuild), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("/api/SanPhamChiTiet/Ngung_Kinh_Doanh_List_SanPham", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<bool> UpdateAynsc(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("/api/SanPhamChiTiet/Update-SanPhamChiTiet", sanPhamChiTietDTO);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                throw new Exception("Not IsSuccessStatusCode");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<List<SanPhamChiTietExcelViewModel>> GetListSanPhamExcelAynsc()
        {
            return (await _httpClient.GetFromJsonAsync<List<SanPhamChiTietExcelViewModel>>("/api/SanPhamChiTiet/get_list_SanPhamExcel"))!;
        }

        public async Task<SanPhamChiTietDTO> GetItemExcelAynsc(BienTheDTO bienTheDTO)
        {
            try
            {
                var response = (await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/get-ItemExcel", bienTheDTO))!;
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<SanPhamChiTietDTO>();
                }
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }

        }

        public async Task UpDatSoLuongAynsc(SanPhamSoLuongDTO sanPhamSoLuongDTO)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/SanPhamChiTiet/UpdateSoLuong", sanPhamSoLuongDTO);
            Console.WriteLine("___________________________________________________________________________________________________________________________________");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.WriteLine("___________________________________________________________________________________________________________________________________");

        }

        public async Task<List<ItemShopViewModel>?> GetDanhSachBienTheItemShopViewModelAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ItemShopViewModel>?>("/api/SanPhamChiTiet/Get-List-ItemBienTheShopViewModel");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<ItemShopViewModel>();
            }
        }

        public async Task<List<ChatLieu>> GetListModelChatLieuAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<ChatLieu>?>("/api/SanPhamChiTiet/Get-List-ChatLieu"))!;
        }

        public async Task<List<KichCo>> GetListModelKichCoAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<KichCo>?>("/api/SanPhamChiTiet/Get-List-KichCo"))!;
        }

        public async Task<List<KieuDeGiay>> GetListModelKieuDeGiayAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<KieuDeGiay>?>("/api/SanPhamChiTiet/Get-List-KieuDeGiay"))!;
        }

        public async Task<List<LoaiGiay>> GetListModelLoaiGiayAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<LoaiGiay>?>("/api/SanPhamChiTiet/Get-List-LoaiGiay"))!;
        }

        public async Task<List<MauSac>> GetListModelMauSacAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<MauSac>?>("/api/SanPhamChiTiet/Get-List-MauSac"))!;
        }

        public async Task<List<SanPham>> GetListModelSanPhamAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<SanPham>?>("/api/SanPhamChiTiet/Get-List-SanPham"))!;
        }

        public async Task<List<ThuongHieu>> GetListModelThuongHieuAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<ThuongHieu>?>("/api/SanPhamChiTiet/Get-List-ThuongHieu"))!;
        }

        public async Task<List<XuatXu>> GetListModelXuatXuAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<XuatXu>>("/api/SanPhamChiTiet/Get-List-XuatXu"))!;
        }
    }
}
