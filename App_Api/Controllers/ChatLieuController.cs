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
        private readonly IMapper _mapper;
        public ChatLieuController(IMapper mapper, IAllRepo<ChatLieu> repository)
        {
            allRepo = repository;
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
                if (!allRepo.GetAll().Any(x => x.TenChatLieu!.Trim().ToLower() == nameChatLieu && x.IdChatLieu != chatLieuDTO.IdChatLieu))
                {
                    var chatLieu = allRepo.GetAll().First(x => x.IdChatLieu == chatLieuDTO.IdChatLieu);
                    chatLieu.TenChatLieu = chatLieuDTO.TenChatLieu;
                    return allRepo.EditItem(chatLieu);
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
