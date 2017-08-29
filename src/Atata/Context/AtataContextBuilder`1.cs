using System.Collections.Generic;

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

        public AtataContextBuilder<TContext> WithProperties(Dictionary<string, object> propertiesMap)
        {
            propertiesMap.CheckNotNull(nameof(propertiesMap));

            AtataMapper.Map(propertiesMap, Context);

            return this;
        }
    }
}
