using Newtonsoft.Json.Linq;

namespace App_View.Models.Momo
{
    public class MomoRefund
    {
        public string? PartnerCode { get; set; }

        public string? RequestId { get; set; }

        public string? OrderId { get; set; }

        public double? Amount { get; set; }

        public long? TransId { get; set; }

        public int? ResultCode { get; set; }

        public string? Message { get; set; }

        public long? ResponseTime { get; set; }
    }
}
