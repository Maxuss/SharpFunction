using System;

namespace SharpFunction.Universal
{
    /// <summary>
    ///     Class that is used for encapsulating methods
    /// </summary>
    public static class Encapsulator<TValue, TResult>
    {
        /// <summary>
        ///     Encapsulate function
        /// </summary>
        /// <param name="method">Lambda expression to encapsulate</param>
        /// <returns>Encapsulated lambda expression</returns>
        public static Func<TValue, TResult> Encapsulate(Func<TValue, TResult> method)
        {
            return method;
        }
    }

    /// <summary>
    ///     Class that is used for encapsulating methods
    /// </summary>
    public static class Encapsulator<TValue, TExtra, TResult>
    {
        /// <summary>
        ///     Encapsulate function
        /// </summary>
        /// <param name="method">Lambda expression to encapsulate</param>
        /// <returns>Encapsulated lambda expression</returns>
        public static Func<TValue, TExtra, TResult> Encapsulate(Func<TValue, TExtra, TResult> method)
        {
            return method;
        }
    }

    /// <summary>
    ///     Class used for encapsulating void expressions
    /// </summary>
    public static class VoidEncapsulator<V>
    {
        /// <summary>
        ///     Encapsulate void
        /// </summary>
        /// <param name="method">Lambda expression to encapsulate</param>
        /// <returns>Encapsulated lambda expression</returns>
        public static Action<V> Encapsulate(Action<V> method)
        {
            return method;
        }
    }

    /// <summary>
    ///     Class used for encapsulating complex void expressions
    /// </summary>
    public static class VoidEncapsulator<V1, V2>
    {
        /// <summary>
        ///     Encapsulate complex void
        /// </summary>
        /// <param name="method">Lambda expression to encapsulate</param>
        /// <returns>Encapsulated lambda expression</returns>
        public static Action<V1, V2> Encapsulate(Action<V1, V2> method)
        {
            return method;
        }
    }
}