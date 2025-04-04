﻿namespace Atata;

public abstract class ExtraXPathAttribute : MulticastAttribute
{
    protected ExtraXPathAttribute(string? xPath)
    {
        if (xPath is not null)
        {
            RawXPath = xPath;
            XPath = (xPath[0] is '/' or '[') ? xPath : "/" + xPath;
        }
    }

    /// <summary>
    /// Gets the raw XPath.
    /// </summary>
    public string? RawXPath { get; }

    /// <summary>
    /// Gets the XPath prepended with <c>"/"</c>, if it can be applied.
    /// </summary>
    public string? XPath { get; }
}
