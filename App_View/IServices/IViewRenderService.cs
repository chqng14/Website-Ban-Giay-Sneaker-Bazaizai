
namespace App_View.IServices
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
        Task<string> RenderToStringAsync(string viewName);

    }
}
