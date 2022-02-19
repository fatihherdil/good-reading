namespace GoodReading.Application.ResponseModels
{
    public interface IResponse<T>
    {
        string Status { get; set; }

        int ResponseCode { get; set; }

        T Response { get; set; }
    }
}
