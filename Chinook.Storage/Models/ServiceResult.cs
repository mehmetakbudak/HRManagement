using System.Collections.Generic;
using System.Net;

namespace Chinook.Storage.Models
{
    public class BaseResult
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class ServiceResult : BaseResult
    {
        public string Exception { get; set; }
        public List<string> Exceptions { get; set; }
        public object Data { get; set; }
    }

    public class ServiceReturnModel
    {
        public string Message { get; set; }
        public List<string> Exceptions { get; set; }
        public object Data { get; set; }
    }
}
