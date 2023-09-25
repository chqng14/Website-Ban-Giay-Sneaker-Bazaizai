using App_Data.ViewModels.SanPhamChiTiet;
using App_View.IServices;
using App_Data.Models;
using System.Net.Http.Json;
using App_Data.ViewModels.Anh;
using Newtonsoft.Json;
using System.Text;

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
                var response = await _httpClient.DeleteAsync("/api/SanPhamChiTiet/Creat-SanPhamChiTiet");
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

        public async Task<List<SanPhamChiTietViewModel>> GetListSanPhamChiTietViewModelAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<SanPhamChiTietViewModel>>("/api/SanPhamChiTiet/Get-List-SanPhamChiTiet"))!;
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
