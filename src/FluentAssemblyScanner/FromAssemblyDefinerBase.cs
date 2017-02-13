using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

namespace FluentAssemblyScanner
{
    public abstract class FromAssemblyDefinerBase
    {
        /// <summary>
        ///     The assemblies
        /// </summary>
        protected IEnumerable<Assembly> Assemblies;

        /// <summary>
        ///     The assembly filters
        /// </summary>
        protected Predicate<Assembly> AssemblyFilter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FromAssemblyDefinerBase" /> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        protected internal FromAssemblyDefinerBase(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies;
            AssemblyFilter = assembly => true;
        }

        /// <summary>
        ///     Alls the types.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public abstract IEnumerable<Type> GetAllTypes();

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [NotNull]
        public BasedOnDefiner BasedOn<T>()
        {
            return BasedOn(typeof(T));
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        [NotNull]
        public BasedOnDefiner BasedOn(Type basedOn)
        {
            return BasedOn((IEnumerable<Type>)new[] { basedOn });
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        [NotNull]
        public BasedOnDefiner BasedOn(params Type[] basedOn)
        {
            return BasedOn((IEnumerable<Type>)basedOn);
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        [NotNull]
        public BasedOnDefiner BasedOn(IEnumerable<Type> basedOn)
        {
            return new BasedOnDefiner(basedOn, this);
        }

        /// <summary>
        ///     Picks any.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public BasedOnDefiner PickAny()
        {
            return BasedOn<object>();
        }
    }
}
