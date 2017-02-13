using System;

using JetBrains.Annotations;

namespace FluentAssemblyScanner
{
    internal static class AppDomainExtensions
    {
        [NotNull]
        public static string GetActualDomainPath([NotNull] this AppDomain @this)
        {
            return @this.RelativeSearchPath ?? @this.BaseDirectory;
        }
    }
}
