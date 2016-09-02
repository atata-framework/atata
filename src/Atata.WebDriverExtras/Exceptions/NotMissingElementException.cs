using System;
using System.Runtime.Serialization;
using OpenQA.Selenium;

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

        protected NotMissingElementException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
