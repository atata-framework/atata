namespace Atata
{
    /// <summary>
    /// Specifies the stage of <see cref="AtataContext"/> driver initialization.
    /// </summary>
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
}
