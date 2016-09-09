using System.Globalization;
using System.Text;

namespace Atata
{
    public abstract class TextOutputLogConsumer : ILogConsumer
    {
        public string Separator { get; set; } = " ";

        public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.ffff";

        public void Log(LogEventInfo eventInfo)
        {
            string completeMessage = BuildCompleteMessage(eventInfo);
            Write(completeMessage);
        }

        protected abstract void Write(string completeMessage);

        private string BuildCompleteMessage(LogEventInfo eventInfo)
        {
            StringBuilder builder = new StringBuilder();

            builder.
                Append(eventInfo.Timestamp.ToString(TimestampFormat, CultureInfo.InvariantCulture)).
                Append(Separator).
                Append(eventInfo.Level.ToString(TermCase.Upper)).
                Append(Separator).
                Append(eventInfo.Message);

            if (eventInfo.Exception != null)
                builder.Append(Separator).Append(eventInfo.Exception.ToString());

            return builder.ToString();
        }
    }
}
