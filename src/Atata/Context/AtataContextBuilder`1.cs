namespace Atata
{
    public class AtataContextBuilder<TContext> : AtataContextBuilder
    {
        public AtataContextBuilder(TContext context, AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            Context = context;
        }

        public TContext Context { get; private set; }
    }
}
