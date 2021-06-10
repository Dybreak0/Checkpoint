using System.Net;

namespace MobileJO.Data.ViewModels.Common
{
    public class ResponseViewModel
    {
        public object Data { get; set; }
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
    }
}
