using App_Data.Models;
using App_Data.ViewModels.SanPhamYeuThichDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace App_View.Controllers
{
    public class SanPhamYeuThichController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;

        public SanPhamYeuThichController(HttpClient httpClient, UserManager<NguoiDung> userManager, SignInManager<NguoiDung> signInManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        // GET: SanPhamYeuThichController
        public async Task<ActionResult> DanhSachSanPhamYeuThich()
        {
            var idNguoiDung = _userManager.GetUserId(User);
            var data = await _httpClient.GetFromJsonAsync<List<SanPhamYeuThichViewModel>>($"/api/SanPhamYeuThich/Get-Danh-Sach-SanPhamYeuThich?idNguoiDung={idNguoiDung}");
            return View(data);
        }

        // GET: SanPhamYeuThichController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SanPhamYeuThichController/Create
        [HttpPost]
        [Authorize]
        [Route("SanPhamYeuThich/Create")]
        public async Task<IActionResult> Create([FromBody]SanPhamYeuThichDTO sanPhamYeuThichDTO)
        {
            sanPhamYeuThichDTO.IdNguoiDung = _userManager.GetUserId(User);
            await _httpClient.PostAsJsonAsync("/api/SanPhamYeuThich/add-sanphamyeuthich", sanPhamYeuThichDTO);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("SanPhamYeuThich/Remove")]
        public async Task<IActionResult> Remove([FromBody] SanPhamYeuThichDTO sanPhamYeuThichDTO)
        {
            sanPhamYeuThichDTO.IdNguoiDung = _userManager.GetUserId(User);
            string jsonContent = JsonConvert.SerializeObject(sanPhamYeuThichDTO);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("/api/SanPhamYeuThich/Remove-sanphamyeuthich", UriKind.RelativeOrAbsolute),
                Content = content
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

    }
}
