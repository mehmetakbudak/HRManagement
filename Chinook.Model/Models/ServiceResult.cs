using System.Collections.Generic;
using System.Net;

namespace Chinook.Model.Models
{
    public class ServiceResult
    {       
        public object Data { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public List<string> Exceptions { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public class ServiceReturnModel
    {
        public string Message { get; set; }
        public List<string> Exceptions { get; set; }
        public object Data { get; set; }
    }
}
