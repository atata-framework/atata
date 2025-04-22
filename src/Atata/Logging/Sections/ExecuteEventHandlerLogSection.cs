namespace Atata;

public class ExecuteEventHandlerLogSection : LogSection
{
    public ExecuteEventHandlerLogSection(object eventData, object eventHandler)
    {
        string eventHandlerAsString = Stringifier.ToStringInSimpleStructuredForm(eventHandler);

        Message = $"Execute event handler {eventHandlerAsString} on {eventData.GetType().ToStringInShortForm()}";

        Level = LogLevel.Trace;
    }
}
