//using System;

//using Xunit;

//namespace FluentAssemblyScanner.Tests
//{
//    public class FluentAssemblyScannerTest
//    {
//        [Fact]
//        public void FromAssembly_IfAssemblyIsNull_ThrowsArgumentNullException()
//        {
//            Assert.Throws(typeof(ArgumentNullException), () => AssemblyScanner.FromAssembly(null));
//        }

//        [Fact]
//        public void FromAssembly_ShouldReturnOneType()
//        {
//            var types = AssemblyScanner.FromThisAssembly()
//                                             .IncludeNonPublicTypes()
//                                             .BasedOn<IAnimal>()
//                                             .Filter()
//                                             .MethodHasAttribute<Voice>()
//                                             .Scan();

//            Assert.Equal(types.Count, 1);
//        }

//        [Fact]
//        public void FromAssemblyContaining_IfAssemblyIsNull_ThrowsArgumentNullException()
//        {
//            Assert.Throws(typeof(ArgumentNullException), () => AssemblyScanner.FromAssemblyContaining(null));
//        }

//        [Fact]
//        public void FromAssemblyInDirectory_IfFilterIsNull_ThrowsArgumentNullException()
//        {
//            Assert.Throws(typeof(ArgumentNullException), () => AssemblyScanner.FromAssemblyInDirectory(null));
//        }
//    }

//    public interface IAnimal
//    {
//        IAnimal Walks();
//    }

//    public interface IHuman : IAnimal
//    {
//        IHuman Runs();
//    }

//    public interface IBird : IAnimal
//    {
//        IBird Flies();
//    }

//    public class Human : IHuman
//    {
//        public IAnimal Walks()
//        {
//            return this;
//        }

//        public IHuman Runs()
//        {
//            return this;
//        }
//    }

//    public class Bird : IBird
//    {
//        [Voice]
//        public IAnimal Walks()
//        {
//            return this;
//        }

//        public IBird Flies()
//        {
//            return this;
//        }
//    }

//    public class Voice : Attribute { }
//}