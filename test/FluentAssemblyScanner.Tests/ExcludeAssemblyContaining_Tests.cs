using System.Linq;

using FluentAssemblyScanner.Tests.AdditionalAssembly;
using FluentAssemblyScanner.Tests.SpecClasses;

using FluentAssertions;

using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class ExcludeAssemblyContaining_Tests
    {
        [Fact]
        public void AllTypes_should_return_count_greater_than_zero()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter(string.Empty);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyContaining<IAdditionalService>()
                    .GetAllTypes()
                    .Count()
                    .Should()
                    .BeGreaterThan(0);
        }

        [Fact]
        public void should_work_as_expected()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter(string.Empty);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyContaining<IAdditionalService>()
                    .GetAllTypes()
                    .Should()
                    .Contain(typeof(AbstractDbContext));
        }

        [Fact]
        public void should_work_on_not_wanted_assemblies()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter(string.Empty);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyContaining<IAdditionalService>()
                    .GetAllTypes()
                    .Should()
                    .NotContain(typeof(AdditionalService));
        }

        [Fact]
        public void should_not_contains_private_classes_when_nonpublictypes_is_not_included()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter(string.Empty);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyContaining<IAdditionalService>()
                    .GetAllTypes()
                    .Should().NotContain(typeof(SomePrivateClass));
        }

        [Fact]
        public void should_contains_private_classes_when_nonpublictypes_included()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter(string.Empty);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyContaining<IAdditionalService>()
                    .IncludeNonPublicTypes()
                    .GetAllTypes()
                    .Should().Contain(typeof(SomePrivateClass));
        }

        [Fact]
        public void should_not_find_any_type_from_excluded_assembly()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter(string.Empty);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyContaining<IAdditionalService>()
                    .BasedOn<AdditionalService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        [Fact]
        public void should_not_find_any_type_from_excluded_assembly_with_named()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyFilter = new AssemblyFilter(string.Empty);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInDirectory(assemblyFilter);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.ExcludeAssemblyNamed("FluentAssemblyScanner.Tests.AdditionalAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
                    .BasedOn<IAdditionalService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        private class SomePrivateClass
        {
        }
    }
}
