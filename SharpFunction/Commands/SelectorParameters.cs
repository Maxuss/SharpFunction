using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFunction.Exceptions;
namespace SharpFunction.Commands
{
    /// <summary>
    /// Represents parameters for entity selector
    /// </summary>
    public struct SelectorParameters
    {
        /// <summary>
        /// Represents KeyValue pairs of arguments for selector
        /// </summary>
        public readonly IDictionary<string, string> Arguments { get; }

        /// <summary>
        /// Represents whether selector parameters are empty
        /// </summary>
        internal readonly bool isNull { get; }

        /// <summary>
        /// Create new selector parameters with specified parameters.
        /// </summary>
        /// <param name="params">Input parameters as *key1*, *value1*, *key2*, *value2*, etc.</param>
        /// <exception cref="InvalidSelectorParameters"/>
        public SelectorParameters(params string[] @params)
        {
            isNull = false;
            if(@params.Length % 2 == 0)
            {
                IDictionary<string, string> tmp = new Dictionary<string, string>();
                for(int i = 0; i < @params.Length; i = i + 2) tmp.Add(@params[i], @params[i+1]);
                Arguments = tmp;
            }
            else throw new InvalidSelectorParameters();

        }

        /// <summary>
        /// Create new selector parameters with specified parameters
        /// </summary>
        /// <param name="params">Input parameters as IDictionary</param>
        public SelectorParameters(IDictionary<string, string> @params)
        {
            Arguments = @params;
            isNull = false;
        }

        internal SelectorParameters(string placeholder)
        {
            Arguments = new Dictionary<string, string>();
            isNull = true;
        }

        /// <summary>
        /// Turns Parameters into string for command
        /// </summary>
        /// <returns>Parameters as Minecraft Entity Selector JSON string</returns>
        public string String()
        {
            if (!isNull)
            {
                string s = "[";
                foreach (string k in Arguments.Keys)
                {
                    string v = Arguments[k];
                    s += $"{k}={v}";
                    if (Arguments.Last().Key != k) s += ",";
                }
                s += "]";
                return s;
            }
            else;
            {
                return string.Empty;
            }
        }
    }
}
