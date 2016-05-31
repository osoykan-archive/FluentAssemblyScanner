using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public class BasedOnDescriptor
    {
        private readonly IEnumerable<Assembly> assemblies;
        private readonly List<Type> potentialBasedOns;
        private Predicate<Type> ifFilter;
        private bool includeNonPublicTypes;

        internal BasedOnDescriptor(IEnumerable<Type> basedOn, Predicate<Type> additionalFilters, IEnumerable<Assembly> assemblies)
        {
            potentialBasedOns = basedOn.ToList();
            this.assemblies = assemblies;
            If(additionalFilters);
            If(t => t.IsPublic == false);
        }

        public BasedOnDescriptor If(Predicate<Type> filter)
        {
            ifFilter += filter;
            return this;
        }

        public BasedOnDescriptor IncludeNonPublicTypes()
        {
            includeNonPublicTypes = true;
            return this;
        }

        public BasedOnDescriptor OrBasedOn(Type basedOn)
        {
            potentialBasedOns.Add(basedOn);
            return this;
        }

        public ICollection<Type> Scan()
        {
            Type[] baseTypes;
            var types = (from asm in assemblies
                         from type in asm.GetTypes()
                         where Accepts(type, out baseTypes)
                         select type)
                .ToList();

            return types;
        }

        private bool Accepts(Type type, out Type[] baseTypes)
        {
            return IsBasedOn(type, out baseTypes) && ExecuteIfCondition(type);
        }

        private bool ExecuteIfCondition(Type type)
        {
            if (ifFilter == null)
            {
                return true;
            }

            foreach (Predicate<Type> filter in ifFilter.GetInvocationList())
            {
                if (filter(type) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsBasedOn(Type type, out Type[] baseTypes)
        {
            var actuallyBasedOn = new List<Type>();
            foreach (var potentialBase in potentialBasedOns)
            {
                if (potentialBase.IsAssignableFrom(type))
                {
                    actuallyBasedOn.Add(potentialBase);
                }
                else if (potentialBase.IsGenericTypeDefinition)
                {
                    if (potentialBase.IsInterface)
                    {
                        if (IsBasedOnGenericInterface(type, potentialBase, out baseTypes))
                        {
                            actuallyBasedOn.AddRange(baseTypes);
                        }
                    }

                    if (IsBasedOnGenericClass(type, potentialBase, out baseTypes))
                    {
                        actuallyBasedOn.AddRange(baseTypes);
                    }
                }
            }
            baseTypes = actuallyBasedOn.Distinct().ToArray();
            return baseTypes.Length > 0;
        }

        private static bool IsBasedOnGenericClass(Type type, Type basedOn, out Type[] baseTypes)
        {
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == basedOn)
                {
                    baseTypes = new[] { type };
                    return true;
                }

                type = type.BaseType;
            }
            baseTypes = null;
            return false;
        }

        private static bool IsBasedOnGenericInterface(Type type, Type basedOn, out Type[] baseTypes)
        {
            var types = new List<Type>(4);
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType &&
                    @interface.GetGenericTypeDefinition() == basedOn)
                {
                    if (@interface.ReflectedType == null &&
                        @interface.ContainsGenericParameters)
                    {
                        types.Add(@interface.GetGenericTypeDefinition());
                    }
                    else
                    {
                        types.Add(@interface);
                    }
                }
            }
            baseTypes = types.ToArray();
            return baseTypes.Length > 0;
        }
    }
}