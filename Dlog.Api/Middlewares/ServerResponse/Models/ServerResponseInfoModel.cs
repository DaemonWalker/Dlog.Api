using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dlog.Api.Middlewares.ServerResponse.Models
{
    public class ServerResponseInfoModel
    {
        [JsonIgnore]
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; }
        public int StatusCode => (int)HttpStatusCode;
    }
}
