namespace Atata;

/// <summary>
/// Specifies the stage of <see cref="AtataContext"/> driver initialization.
/// </summary>
#warning Rename AtataContextDriverInitializationStage to WebDriverInitializationStage or common SessionInitializationStage; or most likely remove it completely.
public enum AtataContextDriverInitializationStage
{
    /// <summary>
    /// Should not be initialized.
    /// </summary>
    None,

    /// <summary>
    /// Initialize upon build.
    /// </summary>
    Build,

    /// <summary>
    /// Initialize on demand.
    /// </summary>
    OnDemand
}
