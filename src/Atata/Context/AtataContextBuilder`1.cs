namespace Atata
{
    public class AtataContextBuilder<TContext> : AtataContextBuilder, IHasContext<TContext>
    {
        public AtataContextBuilder(TContext context, AtataBuildingContext buildingContext)
            : base(buildingContext) =>
            Context = context;

        /// <summary>
        /// Gets the context.
        /// </summary>
        public TContext Context { get; private set; }
    }
}
