using System;
using System.Net;

namespace Application.ErrorHandler
{
    public class ExceptionHandler : Exception
    {
        public HttpStatusCode Code { get; }
        public object Errors { get; }
        public ExceptionHandler(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }
    }
}