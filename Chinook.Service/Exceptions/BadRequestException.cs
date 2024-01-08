using Chinook.Storage.Models;
using System.Net;

namespace Chinook.Service.Exceptions
{
    public class BadRequestException : ApiExceptionBase
    {
        public BadRequestException() : base()
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.BadRequest, Message = "Geçersiz istek." };
        }

        public BadRequestException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.BadRequest, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
    }
}
