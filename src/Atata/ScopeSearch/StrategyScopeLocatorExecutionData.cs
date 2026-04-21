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

    public IEnumerable<StrategyScopeLocatorLayerExecutionUnit> LayerUnits { get; }

    public StrategyScopeLocatorExecutionUnit FinalUnit { get; }
}
