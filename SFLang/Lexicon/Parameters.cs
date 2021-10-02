using System.Collections;
using System.Globalization;

namespace SFLang.Lexicon
{
    public class Parameters : IEnumerable<object>
    {
        List<object> _list = new List<object>();

        public Parameters()
        { }

        public Parameters(params object[] arguments)
        {
            _list.AddRange(arguments);
        }

        public Parameters(IEnumerable<object> arguments)
        {
            _list.AddRange(arguments);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public void Add(object value)
        {
            _list.Add(value);
        }

        public object Get(int index, object defaultValue = null)
        {
            if (index >= _list.Count)
                return defaultValue;
            return _list[index];
        }

        public T Get<T>(int index, T defaultValue = default(T))
        {
            // Retrieving argument and converting it to type specified by caller.
            var obj = Get(index);
            if (obj == null)
                return defaultValue;
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }
        
        public IEnumerator<object> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}