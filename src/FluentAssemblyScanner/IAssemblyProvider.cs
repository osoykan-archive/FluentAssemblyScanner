using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}