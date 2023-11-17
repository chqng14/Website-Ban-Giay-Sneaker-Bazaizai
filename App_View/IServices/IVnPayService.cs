using App_View.Models;

namespace App_View.IServices;
public interface IVnPayService
{
    Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    Task<string> RePaymentUrl(PaymentInformationModel model, HttpContext context,string idHoaDon);
    Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections);
    Task<PaymentResponseModel> RePaymentExecute(IQueryCollection collections);
}