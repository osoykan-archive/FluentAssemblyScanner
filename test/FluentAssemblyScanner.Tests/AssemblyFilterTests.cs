using System.Linq;
using FluentAssertions;
using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class AssemblyFilterTests
    {
        [Fact]
        public void Should()
        {
            var assemblyFilter = new AssemblyFilter();
            var assemblies = assemblyFilter.GetAssemblies();
            assemblies.Count().Should().Be(3);
        }
    }
}