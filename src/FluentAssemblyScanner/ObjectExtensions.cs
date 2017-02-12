using JetBrains.Annotations;

namespace FluentAssemblyScanner
{
    internal static class ObjectExtensions
    {
        /// <summary>
        ///     Casts the specified this.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this">The this.</param>
        /// <returns></returns>
        [NotNull]
        public static T As<T>([NotNull] this object @this) where T : class
        {
            Check.NotNull(@this, nameof(@this));

            return (T)@this;
        }
    }
}
