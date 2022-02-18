using System.Net;

namespace GoodReading.Application.ResponseModels
{
    public struct DefaultResponse : IResponse
    {
        public string Status { get; set; }
        public int ResponseCode { get; set; }
        public object Response { get; set; }

        public DefaultResponse(string status, int responseCode, object response)
        {
            Status = status;
            ResponseCode = responseCode;
            Response = response;
        }
        public DefaultResponse(int responseCode, object response)
        {

            Status = ((HttpStatusCode) responseCode).ToString();
            ResponseCode = responseCode;
            Response = response;
        }

        public DefaultResponse(string status, object response)
        {
            Status = status;
            ResponseCode = 200;
            Response = response;
        }

        public DefaultResponse(object response)
        {
            Status = HttpStatusCode.OK.ToString();
            ResponseCode = 200;
            Response = response;
        }


        public DefaultResponse(HttpStatusCode statusCode, object response)
        {
            Status = statusCode.ToString();
            ResponseCode = (int) statusCode;
            Response = response;
        }
    }
}
