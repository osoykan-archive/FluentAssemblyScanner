using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public interface IAssemblyDiscoverer
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}