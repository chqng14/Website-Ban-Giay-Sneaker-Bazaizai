using App_Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace App_View.IServices
{
    public interface IThongKeService
    {
        public Task<double> DoanhThuTheoThang(int month);
        public  Task<int> DonDatHangTheoThang(int month);
        public Task<string> ThongKeBanHang();
        public  Task<List<HoaDon>> DonDatHnagGanDay();
        public Task<JsonResult> DoanhThuTrong7ngay();
        public Task<List<HoaDon>> DonDonHangGanDay();
    }
}
