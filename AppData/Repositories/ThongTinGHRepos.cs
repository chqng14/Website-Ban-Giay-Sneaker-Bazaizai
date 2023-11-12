using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.ThongTinGHDTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class ThongTinGHRepos : IThongTinGHRepos
    {
        BazaizaiContext context;
        private readonly IMapper _mapper;
        public ThongTinGHRepos(IMapper mapper)
        {
            _mapper = mapper;
            context = new BazaizaiContext();
        }
        public bool AddThongTinGH(ThongTinGiaoHang item)
        {
            try
            {
                context.thongTinGiaoHangs.Add(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditThongTinGH(ThongTinGiaoHang item)
        {
            try
            {
                //var id = context.thongTinGiaoHangs.Find(item.IdThongTinGH);
                context.thongTinGiaoHangs.Update(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<ThongTinGiaoHang> GetAll()
        {
            return context.thongTinGiaoHangs.ToList();
        }

        public bool RemoveThongTinGH(ThongTinGiaoHang item)
        {
            try
            {
                var id = context.thongTinGiaoHangs.Find(item.IdThongTinGH);
                context.thongTinGiaoHangs.Remove(id);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<ThongTinGHDTO> GetAllDTO()
        {
            var thongtin = context.thongTinGiaoHangs.ToList();
            return _mapper.Map<List<ThongTinGHDTO>>(thongtin);
        }
    }
}
