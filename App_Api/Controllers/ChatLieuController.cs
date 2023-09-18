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
    public class ChatLieuController : ControllerBase
    {
        private readonly IAllRepo<ChatLieu> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<ChatLieu> ChatLieu;
        public ChatLieuController()
        {
            ChatLieu = dbContext.ChatLieus;
            AllRepo<ChatLieu> all = new AllRepo<ChatLieu>(dbContext, ChatLieu);
            allRepo = all;
        }
        // GET: api/<ChatLieuController>
        [HttpGet]
        public IEnumerable<ChatLieu> GetAllChatLieu()
        {
            return allRepo.GetAll();
        }

        // GET api/<ChatLieuController>/5
        [HttpGet("{id}")]
        public ChatLieu GetChatLieuById(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdChatLieu == id);
        }

        // POST api/<ChatLieuController>
        [HttpPost]
        public bool AddChatLieu(string ten, int trangthai)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "CL1";
            }
            else
            {
                ma = "CL" + (allRepo.GetAll().Count() + 1);
            }
            var Chatlieu = new ChatLieu()
            {
                IdChatLieu = Guid.NewGuid().ToString(),
                MaChatLieu = ma,
                TenChatLieu = ten,
                TrangThai = trangthai
            };
            return allRepo.AddItem(Chatlieu);
        }

        // PUT api/<ChatLieuController>/5
        [HttpPut("SuaChatLieu {id}")]
        public bool SuaChatLieu(string id, string ma, string ten, int trangthai)
        {
            var chatlieu = new ChatLieu()
            {
                IdChatLieu = id,
                MaChatLieu = ma,
                TenChatLieu = ten,
                TrangThai = trangthai
            };
            return allRepo.EditItem(chatlieu);
        }
        // DELETE api/<ChatLieuController>/5
        [HttpDelete("XoaChatLieu{id}")]
        public bool Delete(string id)
        {
            var cl = allRepo.GetAll().FirstOrDefault(c => c.IdChatLieu == id);
            return allRepo.RemoveItem(cl);
        }
    }
}
