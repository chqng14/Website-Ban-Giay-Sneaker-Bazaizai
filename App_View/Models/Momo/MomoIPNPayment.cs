
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace App_View.Models.Momo
{
    public class MomoIPNPayment
    {
        public string? PartnerCode { get; set; }

        public string? RequestId { get; set; }

        public string? OrderId { get; set; }

        public string? ExtraData { get; set; }

        public double? Amount { get; set; }

        public long? TransId { get; set; }

        public string? PayType { get; set; }

        public int? ResultCode { get; set; }

        public JArray? RefundTrans { get; set; }

        public string? Message { get; set; }

        public long? ResponseTime { get; set; }

    }
}
