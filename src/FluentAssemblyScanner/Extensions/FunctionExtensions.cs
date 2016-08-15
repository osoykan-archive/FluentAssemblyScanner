using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssemblyScanner.Extensions
{
    public static class FunctionExtensions
    {
        public static IEnumerable<T> Whereify<T>(this IEnumerable<T> @this, IEnumerable<Func<T, bool>> predicateFuncs)
        {
            var enumerator = @this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentObject = enumerator.Current;

                if (predicateFuncs.As<IEnumerable<Func<T, bool>>>().All(x => x.Invoke(currentObject)))
                {
                    yield return currentObject;
                }
            }
        }
    }
}