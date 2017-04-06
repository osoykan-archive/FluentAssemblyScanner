using System;
using System.Collections.Generic;

using FluentAssemblyScanner.Tests.AdditionalAssembly;
using FluentAssemblyScanner.Tests.SpecClasses;
using FluentAssemblyScanner.Tests.SpecClasses.subPaymentMethodNamespace;

using FluentAssertions;

using Xunit;

namespace FluentAssemblyScanner.Tests
{
    public class FromAssemblyDefinerBase_Tests
    {
        [Fact]
        public void PickAny_count_should_be_greater_than_zero()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            // None.

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInThisApplicationDirectory();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.PickAny().Filter().Scan().Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void should_filter_by_namespace()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            // None.

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInThisApplicationDirectory();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.BasedOn<IPaymentMethod>()
                    .InSameNamespaceOf(typeof(IAdditionalAssemblyService))
                    .Filter()
                    .Scan().Count.Should().Be(0);
        }

        [Fact]
        public void when_sub_namespace_search_option_is_not_active_then_the_sub_namespaced_type_doesnt_count_inside_of_scanned_types()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            // None.

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInThisApplicationDirectory();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.BasedOn<IPaymentMethod>()
                    .InSameNamespaceOf<IPaymentMethod>()
                    .Filter()
                    .Scan()
                    .Should().NotContain(typeof(DBankPaymentMethod));
        }

        [Fact]
        public void when_sub_namespace_search_option_is_active_then_the_sub_namespaced_type_counts_inside_of_scanned_types()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            
            // None.

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInThisApplicationDirectory();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.BasedOn<IPaymentMethod>()
                    .InSameNamespaceOf<IPaymentMethod>(true)
                    .Filter()
                    .Scan()
                    .Should().Contain(typeof(DBankPaymentMethod));
        }

        [Fact]
        public void when_use_with_based_ons_and_classes_filter_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            // None.

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInThisApplicationDirectory();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            List<Type> scannedTypes = instance.BasedOn<IPaymentMethod>()
                                              .OrBasedOn<IAdditionalAssemblyService>()
                                              .Filter()
                                              .Classes()
                                              .Scan();

            scannedTypes.Count.Should().Be(4);
        }

        [Fact]
        public void when_use_with_based_ons_and_filter_should_work_and_returns_also_interfaces()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            // None.

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInThisApplicationDirectory();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            List<Type> scannedTypes = instance.BasedOn<IPaymentMethod>()
                                              .OrBasedOn<IAdditionalAssemblyService>()
                                              .Filter()
                                              .Scan();

            scannedTypes.Count.Should().Be(5);
        }

        [Fact]
        public void should_return_related_types_with_based_ons()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            // None.

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            FromAssemblyDefiner instance = AssemblyScanner.FromAssemblyInThisApplicationDirectory();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            instance.BasedOn<IPaymentMethod>()
                    .OrBasedOn<IAdditionalAssemblyService>()
                    .Filter()
                    .Classes()
                    .Scan()
                    .Should().NotContain(typeof(SampleDbContext));
        }
    }
}
