using System.Reflection;

using FluentAssertions;

using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class AssemblyScanner_Test
    {
        [Fact]
        public void from_assembly_should_instatiate()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssembly(thisAssembly);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.Should().NotBeNull();
        }
    }
}
