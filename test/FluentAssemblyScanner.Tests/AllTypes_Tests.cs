using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Tests.SpecClasses;

using FluentAssertions;

using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class AllTypesTests
    {
        [Fact]
        public void AllTypes_should_be_return_greater_than_zero()
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
            instance.GetAllTypes().Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public void AllTypes_should_be_scan_all_classes_in_given_assembly()
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
            instance.GetAllTypes().Should().Contain(typeof(SecurityService));
            instance.GetAllTypes().Should().Contain(typeof(ISecurityService));
            instance.GetAllTypes().Should().Contain(typeof(Product));
            instance.GetAllTypes().Should().Contain(typeof(SampleDbContext));
            instance.GetAllTypes().Should().Contain(typeof(AbstractDbContext));
        }
    }
}
