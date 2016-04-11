using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public static class TypeExtensions
    {
        public static bool IsClassOrNullable(this Type type)
        {
            return type.IsClass || Nullable.GetUnderlyingType(type) != null;
        }

        public static Enum[] GetIndividualEnumFlags(this Type type)
        {
            if (type.IsDefined(typeof(FlagsAttribute), false))
            {
                Enum[] allFlags = Enum.GetValues(type).Cast<Enum>().Where(x => Convert.ToDecimal(x) != 0m).ToArray();
                List<Enum> individualFlags = new List<Enum>(allFlags.Take(1));

                for (int i = 1; i < allFlags.Length; i++)
                {
                    Enum currentFlag = allFlags[i];
                    if (allFlags.Take(i).All(x => !currentFlag.HasFlag(x)))
                        individualFlags.Add(currentFlag);
                }

                return individualFlags.ToArray();
            }
            else
            {
                return Enum.GetValues(type).Cast<Enum>().ToArray();
            }
        }
    }
}
