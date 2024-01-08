using Chinook.Storage.Models;
using System.Net;

namespace Chinook.Service.Exceptions
{
    public class NotAcceptableException : ApiExceptionBase
    {
        public NotAcceptableException() : base()
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.NotAcceptable, Message = "Kabul Edilmeyen İstek." };
        }

        public NotAcceptableException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.NotAcceptable, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotAcceptable;
    }
}
