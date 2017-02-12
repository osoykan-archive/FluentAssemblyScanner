using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    /// <summary>
    /// </summary>
    public abstract class FromAssemblyDefinerBase
    {
        /// <summary>
        ///     The assemblies
        /// </summary>
        protected IEnumerable<Assembly> Assemblies;

        /// <summary>
        ///     The assembly filters
        /// </summary>
        protected Action<IEnumerable<Assembly>> AssemblyFilters;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FromAssemblyDefinerBase" /> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        protected internal FromAssemblyDefinerBase(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies;
        }

        /// <summary>
        ///     Alls the types.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Type> AllTypes();

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public BasedOnDefiner BasedOn<T>()
        {
            return BasedOn(typeof(T));
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        public BasedOnDefiner BasedOn(Type basedOn)
        {
            return BasedOn((IEnumerable<Type>)new[] { basedOn });
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        public BasedOnDefiner BasedOn(params Type[] basedOn)
        {
            return BasedOn((IEnumerable<Type>)basedOn);
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        public BasedOnDefiner BasedOn(IEnumerable<Type> basedOn)
        {
            return new BasedOnDefiner(basedOn, this);
        }

        /// <summary>
        ///     Picks any.
        /// </summary>
        /// <returns></returns>
        public BasedOnDefiner PickAny()
        {
            return BasedOn<object>();
        }
    }
}
