using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class TrangThai
    {
        //Trạng thái bảng phụ
        public enum TrangThaiCoBan
        {
            [Description("Hoạt động")]
            HoatDong = 0,
            [Description("Không hoạt động")]
            KhongHoatDong = 1,
        }
        public enum TrangThaiVoucher
        {
            [Description("Hoạt động")]
            HoatDong = 0,
            [Description("Không hoạt động")]
            KhongHoatDong = 1,
            [Description("Chưa bắt đầu")]
            ChuaBatDau = 2,
            [Description("Số lượng voucher đã hết")]
            HetVoucher = 3,

        }
        public enum TrangThaiLoaiKhuyenMai
        {
            [Description("Giảm giá tiền mặt")]
            TienMat = 0,
            [Description("Giảm giá theo %")]
            PhanTram = 1,
            [Description("Miễn phí ship")]
            Freeship = 2,

        }
        public enum TrangThaiSale 
        {
            [Description("Không thể áp dụng sale")]
            KhongApDungSale = 0,
            [Description("Được áp dụng sale")]
            DuocApDungSale = 1
        }
    }
}
