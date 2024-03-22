namespace App_View.Models.Momo
{
    public class RefundTrans
    {
        public string? orderId { get; set; }

        public double? amount { get; set; }

        public int? resultCode { get; set; }

        public long? transId { get; set; }

        public long? createdTime { get; set; }
    }
}
