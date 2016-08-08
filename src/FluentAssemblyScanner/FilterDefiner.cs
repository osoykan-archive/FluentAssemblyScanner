using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Extensions;
using FluentAssemblyScanner.ExtensionsF;

namespace FluentAssemblyScanner
{
    public class FilterDefiner : FilterDefinerBase
    {
        private readonly List<Func<Type, bool>> filterActions;
        private readonly List<Type> types;

        public FilterDefiner(List<Type> types, List<Func<Type, bool>> filterActions) : base(types)
        {
            this.types = types;
            this.filterActions = filterActions;

            filterActions.Add(type => AndFilter(type));
            filterActions.Add(type => type.GetMethods().Any(method => MethodFilters.ApplyTo(method)));
        }

        public override List<Type> Scan()
        {
            return types.Whereify(filterActions)
                        .ToList();
        }

        public FilterDefiner Classes()
        {
            Where(type => type.IsClass && type.IsAbstract == false);
            return this;
        }

        public FilterDefiner MethodHasAttribute<TAttribute>() where TAttribute : Attribute
        {
            return MethodHasAttribute(typeof(TAttribute));
        }

        public FilterDefiner MethodHasAttribute(Type attributeType)
        {
            MethodFilters += method => method.GetCustomAttributes(attributeType).Any();
            return this;
        }

        public FilterDefiner MethodName(string methodName)
        {
            MethodFilters += method => method.Name == methodName;
            return this;
        }

        public FilterDefiner MethodNameContains(string methodText)
        {
            MethodFilters += method => method.Name.Contains(methodText);
            return this;
        }

        public FilterDefiner NonStatic()
        {
            Where(type => type.IsAbstract == false && type.IsSealed == false);
            return this;
        }
    }
}