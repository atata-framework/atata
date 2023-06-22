using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Atata
{
    [Serializable]
    public class TypeNotFoundException : Exception
    {
        public TypeNotFoundException()
        {
        }

        public TypeNotFoundException(string message)
            : base(message)
        {
        }

        public TypeNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TypeNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public static TypeNotFoundException For(string typeName, IEnumerable<Assembly> assembliesToFindIn) =>
            new($"Failed to find \"{typeName}\" type. Tried to find in assemblies: {string.Join(", ", assembliesToFindIn.Select(x => x.GetName().Name))}.");
    }
}
