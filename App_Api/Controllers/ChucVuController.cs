using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChucVuController : ControllerBase
    {
        private readonly IAllRepo<ChucVu> iRepos;

        public ChucVuController(IAllRepo<ChucVu> _iRepos)
        {
            iRepos = _iRepos;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = iRepos.GetAll().ToList();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(string TenRole)
        {
            ChucVu role = new ChucVu();
            Random random = new Random();
            role.IdChucVu = Guid.NewGuid().ToString();
            role.TenChucVu = TenRole;
            role.MaChucVu = "ROLE" + random.Next(100, 999).ToString();
            role.TrangThai = 0;
            var result = iRepos.AddItem(role);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, string TenRole, int TrangThai)
        {
            var role = iRepos.GetAll().FirstOrDefault(c => c.IdChucVu == id);
            role.TenChucVu = TenRole;
            role.TrangThai = TrangThai;
            var result = iRepos.EditItem(role);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = iRepos.GetAll().FirstOrDefault(c => c.IdChucVu == id);
            var result = iRepos.RemoveItem(role);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ChucVu> GetById(string id)
        {
            var role = iRepos.GetAll().FirstOrDefault(c => c.IdChucVu == id);
            return role;
        }
        [HttpGet("[action]")]
        public async Task<ChucVu> GetRoleByName(string TenRole)
        {
            var role = iRepos.GetAll().Where(c => c.TenChucVu.Contains(TenRole)).FirstOrDefault();
            return role;
        }
    }
}
