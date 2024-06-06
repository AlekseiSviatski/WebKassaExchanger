using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebKassa.Models.Response
{
    public class WebKassaException
    {
        [JsonPropertyName("exception")]
        public KassaException Exception { get; set; }
        public int StatusCode { get; set; }
    }
    public class KassaException
    {
        [JsonPropertyName("errCodeForClient")]
        public int ErrorCodeForClient { get; set; }

        [JsonPropertyName("errMessForClient")]
        public string ErrorMessageForClient { get; set; }

        [JsonPropertyName("errCodeOriginal")]
        public string ErrorCodeOriginal { get; set; }

        [JsonPropertyName("errMessOriginal")]
        public string ErrorMessageOriginal { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
