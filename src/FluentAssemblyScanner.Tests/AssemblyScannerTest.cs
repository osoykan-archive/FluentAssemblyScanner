#region License
#endregion

namespace FluentAssemblyScanner.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class AssemblyScannerTest
    {
        [Fact]
        public void FromAssembly_IfAssemblyIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => AssemblyScanner.FromAssembly(null));
        }
    }
}
