using App_Data.Models;
using App_Data.ViewModels.KhuyenMaiChiTietDTO;
using App_View.IServices;
using Microsoft.AspNetCore.Http;
using OpenXmlPowerTools;

namespace App_View.Services
{
    public class KhuyenMaiservices: IKhuyenMaiservices
    {
        HttpClient _httpClient;
        public KhuyenMaiservices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<KhuyenMai>> GetAllKhuyenMai()
        {
            var lstKM = (await _httpClient.GetFromJsonAsync<List<KhuyenMai>>("https://localhost:7038/api/KhuyenMai"));
            return lstKM;
        }

        public async Task<bool> CreateKhuyenMai(KhuyenMai khuyenMai,IFormFile formFile)
        {
            var _httpClient = new HttpClient();
            try
            {
                string apiUrl = "/api/KhuyenMai/Create-KhuyenMai";
                var httpClient = new HttpClient();

                var content = new MultipartFormDataContent();
                content.Add(new StringContent(khuyenMai.MaKhuyenMai), "ma");
                content.Add(new StringContent(khuyenMai.TenKhuyenMai), "ten");
                content.Add(new StringContent(khuyenMai.NgayBatDau.ToString()), "ngaybatdau");
                content.Add(new StringContent(khuyenMai.NgayKetThuc.ToString()), "ngayketthuc");
                content.Add(new StringContent(khuyenMai.LoaiHinhKM.ToString()), "LoaiHinhKm");
                content.Add(new StringContent(khuyenMai.MucGiam.ToString()), "mucgiam");

                if (formFile != null && formFile.Length > 0)
                {

                    var streamContent = new StreamContent(formFile.OpenReadStream());
                    streamContent.Headers.Add("Content-Type", formFile.ContentType);
                    content.Add(streamContent, "formFile", formFile.FileName);

                    var response = await _httpClient.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode);
                        return false;
                    }
                }
                return false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
