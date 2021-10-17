using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using SFLang.Language;

namespace SFLang.Lexicon
{
    public class Parameters : IEnumerable<object>
    {
        internal List<object> _list = new();

        public Parameters()
        {
        }

        public Parameters(params object[] arguments)
        {
            _list.AddRange(arguments);
        }

        public Parameters(IEnumerable<object> arguments)
        {
            _list.AddRange(arguments);
        }

        public int Count => _list.Count;

        public object this[int index] => Get(index);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Pop(int index = 0)
        {
            var obj = Get(index);
            _list.RemoveAt(index);
            return obj ?? default;
        }

        public void Add(object value)
        {
            _list.Add(value);
        }

        public object Get(int index, object defaultValue = null)
        {
            var obj = index >= _list.Count ? defaultValue : _list[index];
            return obj is Constant cst ? cst.Value : obj;
        }

        private readonly List<Type> allowedTypes = new()
        {
            typeof(int),
            typeof(long),
            typeof(double),
            typeof(float)
        };
        public T Get<T>(int index, T defaultValue = default)
        {
            // Retrieving argument and converting it to type specified by caller.
            var obj = Get(index);
            if (obj == null)
                return defaultValue;
            if (obj is Constant con && con.Value.GetType() != typeof(T) || (obj.GetType() != typeof(T) && !allowedTypes.Contains(typeof(T)) && !allowedTypes.Contains(obj.GetType())))
            {
                throw new ArgumentException($"Expected parameter of type {typeof(T)} but instead got {obj.GetType()}");
            }
            return (T) Convert.ChangeType(obj is Constant cst ? cst.Value : obj, typeof(T), CultureInfo.InvariantCulture);
        }


        public IEnumerator<object> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}