using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Tests.AdditionalAssembly;
using FluentAssemblyScanner.Tests.SpecClasses;

using FluentAssertions;

using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class Scaning_Tests
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
            instance.AllTypes().Count().Should().BeGreaterThan(0);
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
            instance.AllTypes().Should().Contain(typeof(SecurityService));
            instance.AllTypes().Should().Contain(typeof(ISecurityService));
            instance.AllTypes().Should().Contain(typeof(Product));
            instance.AllTypes().Should().Contain(typeof(SampleDbContext));
            instance.AllTypes().Should().Contain(typeof(AbstractDbContext));
        }

        [Fact]
        public void ExcludeAssemblyContaining_should_work_as_expected()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter("bin");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyContaining<IAdditionalService>().AllTypes().Should().NotContain(typeof(AbstractDbContext));
        }

        [Fact]
        public void ExcludeAssemblyContaining_should_work_on_not_wanted_assemblies()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter("bin");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyContaining<IAdditionalService>().AllTypes().Should().NotContain(typeof(AdditionalService));
        }
    }
}
