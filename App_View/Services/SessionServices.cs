using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;
using App_View.Models.Order;
using Newtonsoft.Json;

namespace App_View.Services
{
    public class SessionServices
    {
        public static List<GioHangChiTietDTO> GetObjFomSession(ISession session, string key)
        {
            //lấy string
            var JsonData = session.GetString(key);
            if (JsonData == null) return new List<GioHangChiTietDTO>();
            //chuyển dữ liệu
            var GioHangsession = JsonConvert.DeserializeObject<List<GioHangChiTietDTO>>(JsonData);
            return GioHangsession;
        }
        public static void SetObjToSession(ISession session, string key, List<GioHangChiTietDTO> value)
        {
            var JsonData = JsonConvert.SerializeObject(value);
            session.SetString(key, JsonData);
        }
        public static void SetspToSession(ISession session, string key, HoaDonTest value)
        {
            var JsonData = JsonConvert.SerializeObject(value);
            session.SetString(key, JsonData);
        }
        public static HoaDonTest GetFomSession(ISession session, string key)
        {
            //lấy string
            var JsonData = session.GetString(key);
            if (JsonData == null) return new HoaDonTest();
            //chuyển dữ liệu
            var GioHangsession = JsonConvert.DeserializeObject<HoaDonTest>(JsonData);
            return GioHangsession;
        }
        public static void SetIdToSession(ISession session, string key, string value)
        {
            var JsonData = JsonConvert.SerializeObject(value);
            session.SetString(key, JsonData);
        }
        public static string GetIdFomSession(ISession session, string key)
        {
            //lấy string
            var JsonData = session.GetString(key);
            if (JsonData == null)
            {
                return null;
            }
            //chuyển dữ liệu
            var GioHangsession = JsonConvert.DeserializeObject<string>(JsonData);
            return GioHangsession;
        }

        public static void SetIPNToSession(ISession session, string key, OrderInfoModel value)
        {
            var JsonData = JsonConvert.SerializeObject(value);
            session.SetString(key, JsonData);
        }
        public static OrderInfoModel GetIPNFomSession(ISession session, string key)
        {
            //lấy string
            var JsonData = session.GetString(key);
            //chuyển dữ liệu
            var GioHangsession = JsonConvert.DeserializeObject<OrderInfoModel>(JsonData);
            return GioHangsession;
        }
        public static List<string> GetLstString(ISession session, string key)
        {
            //lấy string
            var JsonData = session.GetString(key);
            if (JsonData == null) return new List<string>();
            //chuyển dữ liệu
            var lstString = JsonConvert.DeserializeObject<List<string>>(JsonData);
            return lstString;
        }
        public static void SetLstString(ISession session, string key, List<string> value)
        {
            var JsonData = JsonConvert.SerializeObject(value);
            session.SetString(key, JsonData);
        }
    }
}
