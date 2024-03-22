namespace App_View.Models.Order;

public class OrderInfoModel
{
    public string? IdHoaDon { get; set; }
    public string? FullName { get; set; }
    public string? OrderId { get; set; }
    public string? OrderInfo { get; set; }
    public double? Amount { get; set; }
    public string? description { get; set; }
    public long? transId { get; set; }
}