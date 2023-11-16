using System.Security.Cryptography;
using System.Text;
using App_View.IServices;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using App_View.Models.Momo;
using App_View.Models.Order;
using RestSharp;
using DocumentFormat.OpenXml.Wordprocessing;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X9;
using System.Net;
using Newtonsoft.Json.Linq;
using Hangfire.Logging;
using static Google.Apis.Requests.BatchRequest;

namespace App_View.Services;

public class MomoService : IMomoService
{
    private readonly IOptions<MomoOptionModel> _options;

    public MomoService(IOptions<MomoOptionModel> options)
    {
        _options = options;
    }

    public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model, string idHoaDon)
    {
        //model.OrderId = DateTime.UtcNow.Ticks.ToString();
        //model.OrderInfo = "Khách hàng: " + model.FullName + ". Nội dung: " + model.OrderInfo;

        //Thêm idHoaDon vào returnUrl
        //var returnUrlWithIdHoaDon = $"{_options.Value.redirectUrl}?idHoaDon={idHoaDon}";

        //var rawData =
        //    $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={model.OrderId}&amount={model.Amount}&orderId={model.OrderId}&orderInfo={model.OrderInfo}&returnUrl={returnUrlWithIdHoaDon}&notifyUrl={_options.Value.ipnUrl}&extraData=";

        //var testAIO = $"accessKey={_options.Value.AccessKey}&amount={model.Amount}&extraData=&ipnUrl={_options.Value.ipnUrl}&orderId={model.OrderId}&orderInfo={model.OrderInfo}&partnerCode={_options.Value.PartnerCode}&redirectUrl={_options.Value.redirectUrl}&requestId={model.OrderId}& requestType={_options.Value.RequestType}";

        string rawHash = "accessKey=" + _options.Value.AccessKey +
                "&amount=" + model.Amount +
                "&extraData=" + "" +
                "&ipnUrl=" + _options.Value.ipnUrl +
                "&orderId=" + model.OrderId +
                "&orderInfo=" + model.OrderInfo +
                "&partnerCode=" + _options.Value.PartnerCode +
                "&redirectUrl=" + _options.Value.redirectUrl +
                "&requestId=" + model.OrderId +
                "&requestType=" + _options.Value.RequestType
                ;
        var signature = ComputeHmacSha256(rawHash, _options.Value.SecretKey);

        var client = new RestClient(_options.Value.endpoint);
        var request = new RestRequest() { Method = Method.Post };
        request.AddHeader("Content-Type", "application/json; charset=UTF-8");
        // Create an object representing the request data
        var requestData = new
        {
            partnerCode = _options.Value.PartnerCode,
            requestId = model.OrderId,
            amount = model.Amount,
            orderId = model.OrderId,
            orderInfo = model.OrderInfo,
            redirectUrl = _options.Value.redirectUrl,
            ipnUrl = _options.Value.ipnUrl,
            lang = _options.Value.lang,
            extraData = "",
            requestType = _options.Value.RequestType,
            signature = signature
        };
        request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

        int timeoutInMinutes = 30; // Thời gian timeout là 30 phút
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMinutes(timeoutInMinutes));

        var response = await client.ExecuteAsync(request, cts.Token);

        #region test
        //JObject message = new JObject
        //    {
        //        { "partnerCode", _options.Value.PartnerCode },
        //        { "requestId", model.OrderId },
        //        { "amount", model.Amount },
        //        { "orderId", model.OrderId },
        //        { "orderInfo", model.OrderInfo },
        //        { "redirectUrl", _options.Value.redirectUrl },
        //        { "ipnUrl", _options.Value.ipnUrl },
        //        { "lang", _options.Value.lang },
        //        { "extraData", "" },
        //        { "requestType", _options.Value.RequestType },
        //        { "signature", signature }

        //    };

        //HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(_options.Value.endpoint);

        //var postData = message.ToString();

        //var data = Encoding.UTF8.GetBytes(postData);

        //httpWReq.ProtocolVersion = HttpVersion.Version11;
        //httpWReq.Method = "POST";
        //httpWReq.ContentType = "application/json";

        //httpWReq.ContentLength = data.Length;
        //httpWReq.ReadWriteTimeout = 30000;
        //httpWReq.Timeout = 15000;
        //Stream stream = httpWReq.GetRequestStream();
        //stream.Write(data, 0, data.Length);
        //stream.Close();

        //HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

        //string jsonresponse = "";

        //using (var reader = new StreamReader(response.GetResponseStream()))
        //{

        //    string temp = null;
        //    while ((temp = reader.ReadLine()) != null)
        //    {
        //        jsonresponse += temp;
        //    }
        //}
        #endregion

        return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
    }

    public async Task<MomoIPNPayment> IPN(OrderInfoModel model)
    {
        string rawHash = "accessKey=" + _options.Value.AccessKey +
                "&orderId=" + model.OrderId +
                "&partnerCode=" + _options.Value.PartnerCode +
                "&requestId=" + model.OrderId;
        var signature = ComputeHmacSha256(rawHash, _options.Value.SecretKey);

        var client = new RestClient("https://test-payment.momo.vn/v2/gateway/api/query");
        var request = new RestRequest() { Method = Method.Post };
        request.AddHeader("Content-Type", "application/json; charset=UTF-8");

        // Create an object representing the request data
        var requestData = new
        {
            partnerCode = _options.Value.PartnerCode,
            requestId = model.OrderId,
            orderId = model.OrderId,
            signature = signature,
            lang = _options.Value.lang
        };
        request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);
        return JsonConvert.DeserializeObject<MomoIPNPayment>(response.Content);
    }

    public async Task<MomoRefund> Refund(OrderInfoModel model)
    {
        string orderId = "HoanTien" + model.OrderId;
        string rawHash = "accessKey=" + _options.Value.AccessKey +
                "&amount=" + model.Amount +
                "&description=" + model.description +
                "&orderId=" + orderId +
                "&partnerCode=" + _options.Value.PartnerCode +
                "&requestId=" + orderId +
                "&transId=" + model.transId;
        var signature = ComputeHmacSha256(rawHash, _options.Value.SecretKey);

        var client = new RestClient("https://test-payment.momo.vn/v2/gateway/api/refund");
        var request = new RestRequest() { Method = Method.Post };
        request.AddHeader("Content-Type", "application/json; charset=UTF-8");

        // Create an object representing the request data
        var requestData = new
        {
            partnerCode = _options.Value.PartnerCode,
            orderId = orderId,
            requestId = orderId,
            amount = model.Amount,
            transId = model.transId,
            lang = _options.Value.lang,
            description = model.description,
            signature = signature
        };
        request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);
        return JsonConvert.DeserializeObject<MomoRefund>(response.Content);
    }

    public async Task<MomoCreatePaymentResponseModel> RePaymentAsync(OrderInfoModel model, string idHoaDon)
    {
        //model.OrderId = DateTime.UtcNow.Ticks.ToString();
        //model.OrderInfo = "Khách hàng: " + model.FullName + ". Nội dung: " + model.OrderInfo;

        //Thêm idHoaDon vào returnUrl
        var returnUrl = $"https://localhost:7214/DonHang/CallBack&idHoaDon{idHoaDon}";

        //var rawData =
        //    $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={model.OrderId}&amount={model.Amount}&orderId={model.OrderId}&orderInfo={model.OrderInfo}&returnUrl={returnUrlWithIdHoaDon}&notifyUrl={_options.Value.ipnUrl}&extraData=";

        //var testAIO = $"accessKey={_options.Value.AccessKey}&amount={model.Amount}&extraData=&ipnUrl={_options.Value.ipnUrl}&orderId={model.OrderId}&orderInfo={model.OrderInfo}&partnerCode={_options.Value.PartnerCode}&redirectUrl={_options.Value.redirectUrl}&requestId={model.OrderId}& requestType={_options.Value.RequestType}";

        string rawHash = "accessKey=" + _options.Value.AccessKey +
                "&amount=" + model.Amount +
                "&extraData=" + "" +
                "&ipnUrl=" + _options.Value.ipnUrl +
                "&orderId=" + model.OrderId +
                "&orderInfo=" + model.OrderInfo +
                "&partnerCode=" + _options.Value.PartnerCode +
                "&redirectUrl=" + returnUrl +
                "&requestId=" + model.OrderId +
                "&requestType=" + _options.Value.RequestType
                ;
        var signature = ComputeHmacSha256(rawHash, _options.Value.SecretKey);

        var client = new RestClient(_options.Value.endpoint);
        var request = new RestRequest() { Method = Method.Post };
        request.AddHeader("Content-Type", "application/json; charset=UTF-8");
        // Create an object representing the request data
        var requestData = new
        {
            partnerCode = _options.Value.PartnerCode,
            requestId = model.OrderId,
            amount = model.Amount,
            orderId = model.OrderId,
            orderInfo = model.OrderInfo,
            redirectUrl = returnUrl,
            ipnUrl = _options.Value.ipnUrl,
            lang = _options.Value.lang,
            extraData = "",
            requestType = _options.Value.RequestType,
            signature = signature
        };
        request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

        int timeoutInMinutes = 30; // Thời gian timeout là 30 phút
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMinutes(timeoutInMinutes));

        var response = await client.ExecuteAsync(request, cts.Token);


        return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
    }

    private string ComputeHmacSha256(string message, string secretKey)
    {
        byte[] keyByte = Encoding.UTF8.GetBytes(secretKey);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        using (var hmacsha256 = new HMACSHA256(keyByte))
        {
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            string hex = BitConverter.ToString(hashmessage);
            hex = hex.Replace("-", "").ToLower();
            return hex;
        }
    }

}