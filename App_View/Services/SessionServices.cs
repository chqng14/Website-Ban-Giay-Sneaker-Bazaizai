using App_Data.ViewModels.GioHangChiTiet;
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
    }
}
