namespace Atata;

public class StrategyScopeLocatorExecutionData
{
    public StrategyScopeLocatorExecutionData(
        UIComponent component,
        ScopeSource scopeSource,
        IEnumerable<StrategyScopeLocatorLayerExecutionUnit> layerUnits,
        StrategyScopeLocatorExecutionUnit finalUnit)
    {
        Component = component;
        ScopeSource = scopeSource;
        LayerUnits = layerUnits;
        FinalUnit = finalUnit;
    }

    public UIComponent Component { get; }

    public ScopeSource ScopeSource { get; }

    [Obsolete("Use FinalUnit.SearchOptions.IsSafely instead.")] // Obsolete since v3.11.0.
    public bool IsSafely =>
        FinalUnit.SearchOptions.IsSafely;

    public IEnumerable<StrategyScopeLocatorLayerExecutionUnit> LayerUnits { get; }

    public StrategyScopeLocatorExecutionUnit FinalUnit { get; }
}
