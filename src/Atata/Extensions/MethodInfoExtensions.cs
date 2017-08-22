using System.Reflection;

namespace Atata
{
    public static class MethodInfoExtensions
    {
        public static object InvokeStatic(this MethodInfo method, params object[] args)
        {
            return method.Invoke(null, args);
        }

        public static TResult InvokeStatic<TResult>(this MethodInfo method, params object[] args)
        {
            return (TResult)method.Invoke(null, args);
        }
    }
}
