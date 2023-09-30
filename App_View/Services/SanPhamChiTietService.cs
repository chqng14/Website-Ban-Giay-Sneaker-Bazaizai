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
    public class SanPhamChiTietService : ISanPhamChiTietService
    {
        private readonly HttpClient _httpClient;

        public SanPhamChiTietService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseCreataDTO> AddAysnc(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/Creat-SanPhamChiTiet", sanPhamChiTietDTO);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ResponseCreataDTO>();
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
            return await _httpClient.GetFromJsonAsync<SanPhamChiTiet?>("/api/SanPhamChiTiet/Get-SanPhamChiTiet/{id}");
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


        public async Task<List<SanPhamChiTietViewModel>> GetListSanPhamChiTietViewModelAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<SanPhamChiTietViewModel>>("/api/SanPhamChiTiet/Get-List-SanPhamChiTietViewModel"))!;
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
    }
}
