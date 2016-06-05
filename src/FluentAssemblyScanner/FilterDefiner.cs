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

        public override List<Type> Scan()
        {
            return types
                .Where(type => AndFilter.Invoke(type))
                .Where(ApplyMethodFilter)
                .ToList();
        }

        public AndConstraint<FilterDefiner> Classes()
        {
            Where(type => type.IsClass && type.IsAbstract == false);
            return new AndConstraint<FilterDefiner>(this);
        }

        public AndConstraint<FilterDefiner> MethodHasAttribute<TAttribute>() where TAttribute : Attribute
        {
            return MethodHasAttribute(typeof(TAttribute));
        }

        public AndConstraint<FilterDefiner> MethodHasAttribute(Type attributeType)
        {
            MethodFilter += method => method.GetCustomAttributes(attributeType).Any();
            return new AndConstraint<FilterDefiner>(this);
        }

        public AndConstraint<FilterDefiner> MethodName(string methodName)
        {
            MethodFilter += method => method.Name == methodName;
            return new AndConstraint<FilterDefiner>(this);
        }

        public AndConstraint<FilterDefiner> MethodNameContains(string methodText)
        {
            MethodFilter += method => method.Name.Contains(methodText);
            return new AndConstraint<FilterDefiner>(this);
        }

        public AndConstraint<FilterDefiner> NonStatic()
        {
            Where(type => type.IsAbstract == false && type.IsSealed == false);
            return new AndConstraint<FilterDefiner>(this);
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