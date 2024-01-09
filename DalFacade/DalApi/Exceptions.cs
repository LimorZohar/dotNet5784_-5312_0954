using System.Runtime.Serialization;

namespace DalApi
{
    [Serializable]
    public class DalNotExistException : Exception
    {
            public DalNotExistException(string? message) : base(message)
        {
        }

        public DalNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DalNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class DalAlreadytExistException : Exception
    {
        public DalAlreadytExistException(string? message) : base(message)
        {
        }

        public DalAlreadytExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DalAlreadytExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}