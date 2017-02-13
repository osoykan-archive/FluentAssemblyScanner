using System;
using System.Collections.Generic;

using FluentAssemblyScanner.Tests.SpecClasses;

using FluentAssertions;

using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class FilterDefiner_Tests
    {
        [Fact]
        public void when_use_with_filter_with_methodHasAttribute_should_work()
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
            List<Type> scannedTypes = instance.BasedOn<IPaymentMethod>()
                                              .Filter()
                                              .MethodHasAttribute<UnitOfWorkAttribute>()
                                              .Scan();

            scannedTypes.Count.Should().Be(1);
            scannedTypes.Should().Contain(typeof(CBankPaymentMethod));
        }

        [Fact]
        public void when_use_with_filter_with_method_name_contains()
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
            List<Type> scannedTypes = instance.BasedOn<IPaymentMethod>()
                                              .Filter()
                                              .MethodNameContains("BPaymentInternal")
                                              .Scan();

            scannedTypes.Count.Should().Be(1);
            scannedTypes.Should().Contain(typeof(BBankPaymentMethod));
        }

        [Fact]
        public void when_use_with_filter_with_non_abstract_classes_should_not_return_any_abstract()
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
            List<Type> scannedTypes = instance.PickAny()
                                              .Filter()
                                              .Classes()
                                              .NonAbstract()
                                              .Scan();

            scannedTypes.Should().NotContain(typeof(AbstractDbContext));
        }

        [Fact]
        public void when_use_with_filter_with_static_classes_should_return_also_static_classes()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const string assemblyName = "FluentAssemblyScanner.Tests";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyNamed(assemblyName);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            List<Type> scannedTypes = instance.IncludeNonPublicTypes()
                                              .PickAny()
                                              .Filter()
                                              .Classes()
                                              .NonAttribute()
                                              .Scan();

            scannedTypes.Should().Contain(typeof(SomeStaticClass));
        }

        [Fact]
        public void when_use_with_filter_with_non_static_classes_should_not_return_any_static()
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
            List<Type> scannedTypes = instance.PickAny()
                                              .Filter()
                                              .Classes()
                                              .NonStatic()
                                              .Scan();

            scannedTypes.Should().NotContain(typeof(SomeStaticClass));
        }

        [Fact]
        public void when_use_with_filter_with_just_interfaces_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const string assemblyName = "FluentAssemblyScanner.Tests";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyNamed(assemblyName);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            List<Type> scannedTypes = instance.IncludeNonPublicTypes()
                                              .PickAny()
                                              .Filter()
                                              .Interfaces()
                                              .Scan();

            scannedTypes.Count.Should().Be(2);
            scannedTypes.Should().Contain(typeof(ISecurityService));
            scannedTypes.Should().Contain(typeof(ISecurityService));
        }
    }
}
