using System;
using System.Runtime.Serialization;

namespace GoodReading.Domain.Exceptions
{
    [Serializable]
    public abstract class ExceptionBase : Exception
    {

        protected ExceptionBase()
        {
            
        }

        protected ExceptionBase(SerializationInfo serializationInfo, StreamingContext streamingContext):base(serializationInfo, streamingContext)
        {
            
        }

        protected ExceptionBase(string message):base(message)
        {
            
        }

        protected ExceptionBase(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}
