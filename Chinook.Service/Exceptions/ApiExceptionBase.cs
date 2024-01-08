using Chinook.Storage.Models;
using System;
using System.Net;

namespace Chinook.Service.Exceptions
{
    public abstract class ApiExceptionBase : Exception
    {
        public ApiExceptionBase()
        {
        }

        public ApiExceptionBase(string message) : base(message)
        {

        }

        public BaseResult Error { get; set; }

        protected abstract HttpStatusCode HttpStatusCode { get; }

        public int StatusCode => (int)HttpStatusCode;
    }   
}
