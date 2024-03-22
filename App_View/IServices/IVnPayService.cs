using App_View.Models;

namespace App_View.IServices;
public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    string RePaymentUrl(PaymentInformationModel model, HttpContext context,string idHoaDon);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
    PaymentResponseModel RePaymentExecute(IQueryCollection collections);
}