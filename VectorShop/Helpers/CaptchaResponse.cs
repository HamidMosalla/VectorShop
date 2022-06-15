using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace VectorShop.Helpers
{
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}