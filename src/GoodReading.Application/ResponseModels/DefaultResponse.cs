using System.Net;

namespace GoodReading.Application.ResponseModels
{
    public class DefaultResponse<T> : IResponse<T>
    {
        public string Status { get; set; }
        public int ResponseCode { get; set; }
        public T Response { get; set; }

        public DefaultResponse()
        {
        }

        public DefaultResponse(string status, int responseCode, T response)
        {
            Status = status;
            ResponseCode = responseCode;
            Response = response;
        }
        public DefaultResponse(int responseCode, T response)
        {

            Status = ((HttpStatusCode) responseCode).ToString();
            ResponseCode = responseCode;
            Response = response;
        }

        public DefaultResponse(string status, T response)
        {
            Status = status;
            ResponseCode = 200;
            Response = response;
        }

        public DefaultResponse(T response)
        {
            Status = HttpStatusCode.OK.ToString();
            ResponseCode = 200;
            Response = response;
        }


        public DefaultResponse(HttpStatusCode statusCode, T response)
        {
            Status = statusCode.ToString();
            ResponseCode = (int) statusCode;
            Response = response;
        }
    }

    public class DefaultResponse : DefaultResponse<object>
    {
        public DefaultResponse()
        {
        }

        public DefaultResponse(object response) : base(response)
        {
        }

        public DefaultResponse(int responseCode, object response) : base(responseCode, response)
        {
        }

        public DefaultResponse(string status, object response) : base(status, response)
        {
        }

        public DefaultResponse(HttpStatusCode statusCode, object response) : base(statusCode, response)
        {
        }

        public DefaultResponse(string status, int responseCode, object response) : base(status, responseCode, response)
        {
        }
    }
}
