using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.ChatLieuDTO;
using App_Data.ViewModels.KieuDeGiayDTO;
using AutoMapper;
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
        private readonly IMapper _mapper;
        DbSet<ChatLieu> ChatLieu;
        public ChatLieuController(IMapper mapper)
        {
            ChatLieu = dbContext.ChatLieus;
            AllRepo<ChatLieu> all = new AllRepo<ChatLieu>(dbContext, ChatLieu);
            allRepo = all;
            _mapper = mapper;
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
        [HttpPut("sua-chat-lieu")]
        public bool SuaChatLieu(ChatLieuDTO chatLieuDTO)
        {
            try
            {
                var nameChatLieu = chatLieuDTO.TenChatLieu!.Trim().ToLower();
                if (!dbContext.ChatLieus.Where(x => x.TenChatLieu!.Trim().ToLower() == nameChatLieu).Any())
                {
                    var chatLieu = _mapper.Map<ChatLieu>(chatLieuDTO);
                    dbContext.Attach(chatLieu);
                    dbContext.Entry(chatLieu).Property(sp => sp.TenChatLieu).IsModified = true;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // DELETE api/<ChatLieuController>/5
        [HttpDelete("XoaChatLieu/{id}")]
        public bool Delete(string id)
        {
            var cl = allRepo.GetAll().FirstOrDefault(c => c.IdChatLieu == id);
            return allRepo.RemoveItem(cl);
        }
    }
}
