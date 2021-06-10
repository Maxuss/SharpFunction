using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents encapsulator to store more than one type inside a single type param
    /// </summary>
    public sealed class EncapsulatedType<T1, T2, T3>
    {
        /// <summary>
        /// First encapsulated type value
        /// </summary>
        public T1 VALUE1 { get; set; }
        /// <summary>
        /// Second encapsulated type value
        /// </summary>
        public T2 VALUE2 { get; set; }
        /// <summary>
        /// Third encapsulated type value
        /// </summary>
        public T3 VALUE3 { get; set; }
        /// <summary>
        /// Initialize encapsulated type 
        /// </summary>
        /// <param name="v1">First value to store</param>
        /// <param name="v2">Second value to store</param>
        public EncapsulatedType(T1 v1, T2 v2, T3 v3)
        {
            VALUE1 = v1;
            VALUE2 = v2;
            VALUE3 = v3;
        }
    }
}
