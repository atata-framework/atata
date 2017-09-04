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

        /// <summary>
        /// Gets the context.
        /// </summary>
        public TContext Context { get; private set; }

        /// <summary>
        /// Specifies the properties map for the context.
        /// </summary>
        /// <param name="propertiesMap">The properties map.</param>
        /// <returns>The same builder instance.</returns>
        public AtataContextBuilder<TContext> WithProperties(Dictionary<string, object> propertiesMap)
        {
            propertiesMap.CheckNotNull(nameof(propertiesMap));

            AtataMapper.Map(propertiesMap, Context);

            return this;
        }
    }
}
