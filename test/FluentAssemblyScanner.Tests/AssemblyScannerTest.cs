using System;

using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class AssemblyScannerTest
    {
        [Fact]
        public void FromAssembly_IfAssemblyIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => FluentAssemblyScanner.FromAssembly(null));
        }
    }
}