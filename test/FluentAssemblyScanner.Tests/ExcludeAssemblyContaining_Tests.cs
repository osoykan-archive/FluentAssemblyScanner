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
            instance.ExcludeAssemblyContaining<IAdditionalAssemblyService>()
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
            instance.ExcludeAssemblyContaining<IAdditionalAssemblyService>()
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
            instance.ExcludeAssemblyContaining<IAdditionalAssemblyService>()
                    .GetAllTypes()
                    .Should()
                    .NotContain(typeof(AdditionalAssemblyService));
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
            instance.ExcludeAssemblyContaining<IAdditionalAssemblyService>()
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
            instance.ExcludeAssemblyContaining<IAdditionalAssemblyService>()
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
            instance.ExcludeAssemblyContaining<IAdditionalAssemblyService>()
                    .BasedOn<AdditionalAssemblyService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        [Fact]
        public void should_not_find_any_type_from_excluded_assembly_with_full_named()
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
            instance.ExcludeAssemblyFullNamed("FluentAssemblyScanner.Tests.AdditionalAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
                    .BasedOn<IAdditionalAssemblyService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        [Fact]
        public void should_not_find_any_type_from_excluded_assembly_with_name_starts_with()
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
            instance.ExcludeAssemblyNameStartsWith("FluentAssemblyScanner.Tests.Ad")
                    .BasedOn<IAdditionalAssemblyService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        [Fact]
        public void should_not_find_any_type_from_excluded_assembly_with_name_ends_with()
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
            instance.ExcludeAssemblyNameEndsWith("embly")
                    .BasedOn<IAdditionalAssemblyService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        [Fact]
        public void should_not_find_any_type_from_excluded_assembly_with_name()
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
            instance.ExcludeAssemblyNamed("FluentAssemblyScanner.Tests.AdditionalAssembly")
                    .BasedOn<IAdditionalAssemblyService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        [Fact]
        public void should_not_find_any_type_from_excluded_assembly_with_name_contains()
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
            instance.ExcludeAssemblyNameContains("AdditionalAssembly")
                    .BasedOn<IAdditionalAssemblyService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        [Fact]
        public void should_not_find_any_type_from_excluded_assembly_should_work_on_ignored_dynamic_assemblies()
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
            instance.ExcludeAssemblyContaining<IAdditionalAssemblyService>()
                    .IgnoreDynamicAssemblies()
                    .BasedOn<AdditionalAssemblyService>()
                    .Filter()
                    .Scan()
                    .Count.Should().Be(0);
        }

        private class SomePrivateClass
        {
        }
    }
}
