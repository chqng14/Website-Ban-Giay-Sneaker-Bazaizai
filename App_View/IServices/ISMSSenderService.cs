namespace App_View.IServices
{
    public interface ISMSSenderService
    {
        Task SendSmsAsync(string number, string message);
    }
}
