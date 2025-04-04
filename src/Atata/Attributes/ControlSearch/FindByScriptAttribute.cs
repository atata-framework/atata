#nullable enable

namespace Atata;

/// <summary>
/// Specifies that a control should be found by specific script.
/// The script should return an element or collection of elements.
/// The script can also return <c>null</c> for the case when the element is not found.
/// The scope element is passed to the script as an argument and can be used in the script as <c>arguments[0]</c>.
/// </summary>
/// <example>
/// <code>
/// [FindByScript("return document.querySelector('input[type=radio][value=OptionA]')")]
/// public RadioButton&lt;_&gt; OptionA { get; private set; }
/// </code>
/// </example>
public class FindByScriptAttribute : FindAttribute
{
    public FindByScriptAttribute(string script) =>
        Script = script;

    /// <summary>
    /// Gets the script.
    /// </summary>
    public string Script { get; }

    protected override Type DefaultStrategy => typeof(FindByScriptStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return Script;
    }
}
