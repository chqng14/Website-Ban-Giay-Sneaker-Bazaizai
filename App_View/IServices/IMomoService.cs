using App_View.Models.Momo;
using App_View.Models.Order;

namespace App_View.Services;

public interface IMomoService
{
    Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model, string idHoaDon);
    Task<MomoCreatePaymentResponseModel> RePaymentAsync(OrderInfoModel model, string idHoaDon);
    Task<MomoIPNPayment> IPN(OrderInfoModel model);
    Task<MomoRefund> Refund(OrderInfoModel model);
}