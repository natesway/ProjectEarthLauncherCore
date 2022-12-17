using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarthLauncherCore.FileTypes
{
    public class LBLFile : IFile
    {
        public readonly string[] names;
        public readonly string[] values;

        public readonly char Separator;

        public LBLFile(string _path, char _separator) : base(_path)
        {
            Separator = _separator;

            string[] lines = File.ReadAllLines(Path);

            List<string> _names = new List<string>();
            List<string> _values = new List<string>();

            string[] split;
            for (int i = 0; i < lines.Length; i++) {
                split = lines[i].Split(Separator);
                if (split.Length > 1 && split[0] != string.Empty) {
                    _names.Add(split[0]);
                    _values.Add(split[1]);
                }
            }

            names = _names.ToArray();
            values = _values.ToArray();
        }

        public override void Save()
        {
            List<string> lines = new List<string>();

            for (int i = 0; i < names.Length; i++)
                if (names[i] != string.Empty)
                    lines.Add(names[i] + Separator + values[i]);

            File.WriteAllLines(Path, lines.ToArray());
        }

        public string this[string name]
        {
            get => Get(name);
            set => Set(name, value);
        }

        public void Set(string name, string value)
        {
            for (int i = 0; i < names.Length; i++)
                if (names[i] == name) {
                    values[i] = value;
                    return;
                }
        }

        public string Get(string name)
        {
            for (int i = 0; i < names.Length; i++)
                if (names[i] == name)
                    return values[i];

            return string.Empty;
        }
    }
}
