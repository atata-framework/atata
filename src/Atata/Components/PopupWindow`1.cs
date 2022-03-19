using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// <para>
    /// Represents the base class for the popup window page objects.
    /// </para>
    /// <para>
    /// In addition to regular page object attributes, supports:
    /// <list type="bullet">
    /// <item><see cref="WindowTitleElementDefinitionAttribute"/></item>
    /// <item><see cref="WindowTitleAttribute"/></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [PageObjectDefinition(ComponentTypeName = "window", IgnoreNameEndings = "PopupWindow,Window,Popup")]
    public abstract class PopupWindow<TOwner> : PageObject<TOwner>
        where TOwner : PopupWindow<TOwner>
    {
        private string[] _windowTitleValues;

        private TermMatch _windowTitleMatch = TermMatch.Equals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupWindow{TOwner}"/> class.
        /// </summary>
        /// <param name="windowTitleValues">
        /// The window title values.
        /// None can be passed.
        /// </param>
        protected PopupWindow(params string[] windowTitleValues)
        {
            WindowTitleValues = windowTitleValues;
        }

        /// <summary>
        /// Gets or sets the window title values.
        /// By default, the value is taken from <see cref="WindowTitleAttribute"/>.
        /// </summary>
        protected string[] WindowTitleValues
        {
            get => Metadata.TryGet<WindowTitleAttribute>(out var titleAttribute)
                ? titleAttribute.ResolveActualValues(ComponentName)
                : _windowTitleValues;
            set => _windowTitleValues = value;
        }

        /// <summary>
        /// Gets or sets the match that should be used for the window search by the title.
        /// The default value is <see cref="TermMatch.Equals"/>.
        /// By default, the value is taken from <see cref="WindowTitleAttribute"/>.
        /// </summary>
        protected TermMatch WindowTitleMatch
        {
            get => Metadata.TryGet<WindowTitleAttribute>(out var titleAttribute)
                ? titleAttribute.Match
                : _windowTitleMatch;
            set => _windowTitleMatch = value;
        }

        /// <summary>
        /// Gets a value indicating whether window can be found by window title.
        /// Returns <see langword="true"/> when <see cref="WindowTitleValues"/> contains at least one value.
        /// </summary>
        protected bool CanFindByWindowTitle =>
            WindowTitleValues?.Any() ?? false;

        protected override By CreateScopeBy()
        {
            string scopeXPath = Metadata.ComponentDefinitionAttribute?.ScopeXPath ?? "body";

            StringBuilder xPathBuilder = new StringBuilder($".//{scopeXPath}");

            string titleElementXPath = Metadata.Get<WindowTitleElementDefinitionAttribute>()?.ScopeXPath;

            if (CanFindByWindowTitle && !string.IsNullOrWhiteSpace(titleElementXPath))
            {
                xPathBuilder.Append(
                    $"[.//{titleElementXPath}[{WindowTitleMatch.CreateXPathCondition(WindowTitleValues)}]]");
            }

            return By.XPath(xPathBuilder.ToString()).PopupWindow(TermResolver.ToDisplayString(WindowTitleValues)).Visible();
        }
    }
}
