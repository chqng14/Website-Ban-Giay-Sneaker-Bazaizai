using App_View.Models;

namespace App_View.IServices;
public interface IVnPayService
{
    Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections);
}