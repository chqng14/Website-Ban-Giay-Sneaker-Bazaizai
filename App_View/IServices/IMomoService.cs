using App_View.Models.Momo;
using App_View.Models.Order;

namespace App_View.Services;

public interface IMomoService
{
    Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model, string idHoaDon);
    Task<MomoCreatePaymentResponseModel> CreatePayment(OrderInfoModel model, string idHoaDon);
    MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    Task<MomoIPNPayment> IPN(OrderInfoModel model);
    Task<MomoRefund> Refund(OrderInfoModel model);
}