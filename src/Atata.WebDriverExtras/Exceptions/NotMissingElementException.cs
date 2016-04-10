using OpenQA.Selenium;
using System;
using System.Runtime.Serialization;

namespace Atata
{
    [Serializable]
    public class NotMissingElementException : WebDriverException
    {
        public NotMissingElementException()
        {
        }

        public NotMissingElementException(string message)
            : base(message)
        {
        }

        public NotMissingElementException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !NETCF && !SILVERLIGHT && !PORTABLE
        protected NotMissingElementException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
