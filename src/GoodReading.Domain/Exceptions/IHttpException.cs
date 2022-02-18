namespace GoodReading.Domain.Exceptions
{
    public interface IHttpException
    {
        public int StatusCode { get; }
    }
}
