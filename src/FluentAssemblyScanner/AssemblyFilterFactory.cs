using System;

namespace FluentAssemblyScanner
{
    public static class AssemblyFilterFactory
    {
        public static AssemblyFilter All() => new AssemblyFilter(string.Empty);

        public static AssemblyFilter Bin() => new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory);
    }
}