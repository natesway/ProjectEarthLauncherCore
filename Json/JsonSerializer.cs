using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarthLauncherCore.Json
{
    public static class JsonSerializer
    {
        public static JsonObject Deserialize(string text)
        {
            text = text.Substring(text.IndexOf('{'), text.LastIndexOf('}') - text.IndexOf('{'));

            Dictionary<string, object> vals = new Dictionary<string, object>();
            string currentK = string.Empty; // key
            string currentV = string.Empty; // value

            int insideCount = 0;

            byte status = 0;
            for (int i = 0; i < text.Length; i++) {
                char c = text[i];
                if (status == 0 && c == '"') {
                    status = 1;
                }
                else if (status == 1) { // reading name
                    if (c == '"')
                        status = 2;
                    else
                        currentK += c;
                }
                else if (status == 2 && c == ':')
                    status = 3;
                else if (status == 3) { // init value read
                    if (c == '{') {
                        currentV += c;
                        insideCount++;
                        status = 6;
                    }
                    else if (c == '"')
                        status = 4;
                    else if (c != ' ') {
                        currentV += c;
                        status = 7;
                    }
                }
                else if (status == 4) { // reading value
                    if (c == '"') {
                        vals.Add(currentK, currentV);
                        currentK = string.Empty;
                        currentV = string.Empty;
                        status = 5;
                    }
                    else
                        currentV += c;
                }
                else if (status == 7) {
                    if (c == ',' || c == '\n') {
                        vals.Add(currentK, currentV);
                        currentK = string.Empty;
                        currentV = string.Empty;
                        status = 0;
                    }
                    else
                        currentV += c;
                }
                else if (status == 5 && c == ',')
                    status = 0;
                else if (status == 6) { // read json
                    currentV += c;
                    if (c == '{')
                        insideCount++;
                    else if (c == '}')
                        insideCount--;

                    if (insideCount <= 0) {
                        vals.Add(currentK, Deserialize(currentV));
                        currentK = string.Empty;
                        currentV = string.Empty;
                        status = 5;
                    }
                }
            }

            if (currentK != string.Empty)
                vals.Add(currentK, currentV);

            return new JsonObject(vals);
        }
        public static string Serialize(JsonObject obj, JsonSerializationSettings settings)
        {
            string newline = settings.EnterNewLines ? "\n" : "";
            string tab = settings.AddTab ? "\t" : " ";
            string s = "{" + newline;

            if (obj.HasValue) {
                KeyValuePair<string, object>[] kvps = obj.Values.ToArray();
                for (int i = 0; i < kvps.Length - 1; i++) {
                    KeyValuePair<string, object> kvp = kvps[i];

                    if (kvp.Value is JsonObject jo) {
                        string _s = Serialize(jo, settings);
                        string[] lines = _s.Split('\n');
                        for (int j = 1; j < lines.Length; j++)
                            lines[j].Insert(0, tab);

                        s += tab + '"' + kvp.Key + "\": " + kvp.Value + "," + newline;
                    }
                    else if (double.TryParse(kvp.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                        s += tab + '"' + kvp.Key + "\": " + kvp.Value + "," + newline;
                    else
                        s += tab + '"' + kvp.Key + "\": \"" + kvp.Value + "\"," + newline;
                }

                for (int i = kvps.Length - 1; i < kvps.Length; i++) {
                    KeyValuePair<string, object> kvp = kvps[i];
                    if (kvp.Value is JsonObject jo) {
                        string _s = Serialize(jo, settings);
                        string[] lines = _s.Split('\n');
                        for (int j = 1; j < lines.Length; j++) {
                            lines[j] = lines[j].Insert(0, tab.ToString());
                        }

                        s += tab + '"' + kvp.Key + "\": " + string.Join(newline, lines) + newline;
                    }
                    else if (double.TryParse(kvp.Value.ToString(), out _))
                        s += tab + '"' + kvp.Key + "\": " + kvp.Value + newline;
                    else
                        s += tab + '"' + kvp.Key + "\": \"" + kvp.Value + "\"" + newline;
                }
            }

            s += '}';
            return s;
        }
    }
}
