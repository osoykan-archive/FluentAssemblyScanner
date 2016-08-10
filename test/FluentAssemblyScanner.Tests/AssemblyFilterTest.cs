using System;

using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class AssemblyFilterTest
    {
        [Fact]
        public void Ctor_IfDirectoryNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AssemblyFilter(null));
        }

        [Fact]
        public void FilterByAssembly_IfFilterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AssemblyFilter(string.Empty).FilterByAssembly(null));
        }

        [Fact]
        public void FilterByName_IfFilterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AssemblyFilter(string.Empty).FilterByName(null));
        }

        [Fact]
        public void WithKeyToken_IfPublicKeyTokenIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AssemblyFilter(string.Empty).WithKeyToken((byte[])null));
        }
    }
}