using App_Data.ViewModels.GioHangChiTiet;
using App_View.Models.Order;
using Newtonsoft.Json;

namespace App_View.Services
{
    public class SessionServices
    {
        public static List<GioHangChiTietViewModel> GetObjFomSession(ISession session, string key)
        {
            //lấy string
            var JsonData = session.GetString(key);
            if (JsonData == null) return new List<GioHangChiTietViewModel>();
            //chuyển dữ liệu
            var giohangSession = JsonConvert.DeserializeObject<List<GioHangChiTietViewModel>>(JsonData);
            return giohangSession;
        }
        public static void SetObjToSession(ISession session, string key, List<GioHangChiTietViewModel> value)
        {
            var JsonData = JsonConvert.SerializeObject(value);
            session.SetString(key, JsonData);
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
            //chuyển dữ liệu
            var giohangSession = JsonConvert.DeserializeObject<string>(JsonData);
            return giohangSession;
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
            var giohangSession = JsonConvert.DeserializeObject<OrderInfoModel>(JsonData);
            return giohangSession;
        }

        public static void SetListIdToSession(ISession session, string key, List<string> value)
        {
            var JsonData = JsonConvert.SerializeObject(value);
            session.SetString(key, JsonData);
        }
        public static List<string> GetListIdFomSession(ISession session, string key)
        {
            //lấy string
            var JsonData = session.GetString(key);
            //chuyển dữ liệu
            var Session = JsonConvert.DeserializeObject<List<string>>(JsonData);
            return Session;
        }
    }
}
