using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Extensions;

namespace FluentAssemblyScanner
{
    public class BasedOnDefiner : BasedOnDefinerBase
    {
        private readonly FromAssemblyDefinerBase fromAssemblyDefinerBase;

        internal BasedOnDefiner(IEnumerable<Type> basedOns, FromAssemblyDefinerBase fromAssemblyDefinerBase) : base(basedOns)
        {
            this.fromAssemblyDefinerBase = fromAssemblyDefinerBase;
        }

        public BasedOnDefiner UseDefaultfilter()
        {
            If(type => type.IsClass && type.IsAbstract == false);
            return this;
        }

        public override List<Type> Scan()
        {
            return fromAssemblyDefinerBase.SelectedTypes()
                                          .Where(ApplyIfFilter)
                                          .Where(ApplyBasedOnFilter)
                                          .Where(type => ApplyMethodFilter(type.GetMethods()))
                                          .ToList();
        }

        public BasedOnDefiner NonStatic()
        {
            If(type => type.IsAbstract == false && type.IsSealed == false);
            return this;
        }

        public BasedOnDefiner HasAttribute<TAttribute>() where TAttribute : Attribute
        {
            If(Component.HasAttribute<TAttribute>);
            return this;
        }

        protected override bool ApplyBasedOnFilter(Type type)
        {
            return BasedOns.Any(t => t.IsAssignableFrom(type));
        }

        protected override bool ApplyIfFilter(Type type)
        {
            return TypeFilter.ApplyTo(type);
        }

        protected override bool ApplyMethodFilter(MethodInfo method)
        {
            return MethodFilter.ApplyTo(method);
        }

        private bool ApplyMethodFilter(MethodInfo[] methods)
        {
            return methods.Any(ApplyMethodFilter);
        }
    }
}