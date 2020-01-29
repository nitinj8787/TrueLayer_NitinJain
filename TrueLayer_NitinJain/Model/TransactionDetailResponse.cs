using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueLayer_NitinJain.Model
{
    public class TransactionDetailResponse
    {
        [JsonProperty("amount")]
        public string TransactionId { get; set; }

        [JsonProperty("transaction_id")]
        public decimal Amount { get; set; }
    }
}
