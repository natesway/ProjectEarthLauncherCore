using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarthLauncherCore.Json
{
    public class JsonObject
    {
        public Dictionary<string, object> Values;

        public bool HasValue => Values.Count > 0;

        public JsonObject() : this(new Dictionary<string, object>())
        { }
        public JsonObject(string key, object value) : this(new Dictionary<string, object>() { { key, value } })
        { }
        public JsonObject(Dictionary<string, object> _values)
        {
            Values = _values;
        }

        public object this[string key] { get => Values[key]; set => Values[key] = value; }
    }
}
