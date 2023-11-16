﻿using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class KhachHangRepo : IKhachHangRepo
    {
        private BazaizaiContext context;
        public KhachHangRepo()
        {
            context = new BazaizaiContext();    
        }

        public List<KhachHang> GetKhachHangs()
        {
            var list = context.KhachHangs.ToList();
            if (list.Any())
            {
                return list;
            }
            return new List<KhachHang>();
        }

        public string TaoKhachHang(KhachHang khachHang)
        {
            if (context.KhachHangs.FirstOrDefault(c => c.SDT == khachHang.SDT) == null)
            {
                context.KhachHangs.Add(khachHang);
                context.SaveChanges();
                return "Tạo khách hàng thành công";
            }
            return "Số điện thoại đã được sử dụng";
        }
    }
}
