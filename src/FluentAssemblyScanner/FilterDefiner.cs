using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Extensions;

namespace FluentAssemblyScanner
{
    public class FilterDefiner : FilterDefinerBase
    {
        private readonly List<Type> types;

        public FilterDefiner(List<Type> types) : base(types)
        {
            this.types = types;
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
            MethodFilter += method => method.GetCustomAttributes(attributeType).Any();
            return this;
        }

        public FilterDefiner MethodName(string methodName)
        {
            MethodFilter += method => method.Name == methodName;
            return this;
        }

        public FilterDefiner MethodNameContains(string methodText)
        {
            MethodFilter += method => method.Name.Contains(methodText);
            return this;
        }

        public FilterDefiner NonStatic()
        {
            Where(type => type.IsAbstract == false && type.IsSealed == false);
            return this;
        }

        public override List<Type> Scan()
        {
            return types
                .Where(type => AndFilter.Invoke(type))
                .Where(ApplyMethodFilter)
                .ToList();
        }

        private bool ApplyMethodFilter(MethodInfo method)
        {
            return MethodFilter.ApplyTo(method);
        }

        private bool ApplyMethodFilter(Type type)
        {
            return type.GetMethods().Any(ApplyMethodFilter);
        }
    }
}