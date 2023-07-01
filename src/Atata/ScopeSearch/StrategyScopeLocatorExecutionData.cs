namespace Atata;

public class StrategyScopeLocatorExecutionData
{
    public StrategyScopeLocatorExecutionData(
        UIComponent component,
        ScopeSource scopeSource,
        bool isSafely,
        IEnumerable<StrategyScopeLocatorLayerExecutionUnit> layerUnits,
        StrategyScopeLocatorExecutionUnit finalUnit)
    {
        Component = component;
        ScopeSource = scopeSource;
        IsSafely = isSafely;
        LayerUnits = layerUnits;
        FinalUnit = finalUnit;
    }

    public UIComponent Component { get; }

    public ScopeSource ScopeSource { get; }

    public bool IsSafely { get; }

    public IEnumerable<StrategyScopeLocatorLayerExecutionUnit> LayerUnits { get; }

    public StrategyScopeLocatorExecutionUnit FinalUnit { get; }
}
