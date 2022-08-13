using System.Diagnostics;

namespace Atata
{
    public class TraceLogConsumer : TextOutputLogConsumer
    {
        protected override void Write(string completeMessage) =>
            Trace.WriteLine(completeMessage);
    }
}
