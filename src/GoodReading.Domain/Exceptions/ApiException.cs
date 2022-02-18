using System;

namespace GoodReading.Domain.Exceptions
{
    [Serializable]
    public class ApiException : ExceptionBase, IHttpException
    {
        public int StatusCode { get; private set; }

        public ApiException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
