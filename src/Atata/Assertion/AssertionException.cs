using System;
using System.Runtime.Serialization;

namespace Atata
{
    [Serializable]
    public class AssertionException : Exception
    {
        public AssertionException()
        {
        }

        public AssertionException(string message)
            : base(message)
        {
        }

        public AssertionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !NETCF && !SILVERLIGHT && !PORTABLE
        protected AssertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
