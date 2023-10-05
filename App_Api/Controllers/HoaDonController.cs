using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepos hoaDonRepos;
      
        public HoaDonController()
        {
            hoaDonRepos = new HoaDonRepos();
           
        }
        [HttpPost("TaoHoaDonTaiQuay")]
        public void TaoHoaDonTaiQuay()
        {
            hoaDonRepos.TaoHoaDonTaiQuay("181F5D92-FF82-4FDB-B90D-1F82A5E534EE");
        }
    }
}
