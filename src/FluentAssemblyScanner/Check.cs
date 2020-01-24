using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssemblyScanner
{
    [DebuggerStepThrough]
    internal static class Check
    {
        public static T NotNull<T>(T value, string parameterName)
        {
            if (value == null) throw new ArgumentNullException(parameterName);

            return value;
        }

        public static string NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);

            return value;
        }

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string parameterName)
        {
            if (value.IsNullOrEmpty())
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);

            return value;
        }
    }
}