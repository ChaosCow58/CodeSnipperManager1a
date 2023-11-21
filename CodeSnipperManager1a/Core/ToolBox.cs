using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CodeSnipperManager1a.Core
{
    public class ToolBox
    {
        public static void GenerateComboBox(ComboBox comboBox)
        {
            string filePath = @"CodeSnipperManager1a.Jsons.programmingLangs.json";
            string jsonContent = ReadEmbeddedResource(filePath);


            List<Item>? langs = JsonConvert.DeserializeObject<List<Item>>(jsonContent);

            if (langs != null)
            {
                foreach (Item lang in langs)
                {
                    if (lang.type == "programming" && lang.extensions != null && lang.extensions.Length > 0)
                    {

                        comboBox.Items.Add($"{lang.name} ({lang.extensions[0]})");
                    }
                }
            }
        }

        public static string ReadEmbeddedResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Resource '{resourceName}' not found.");
                }

                using (StreamReader r = new StreamReader(stream))
                {
                    return r.ReadToEnd();
                }
            }
        }
    }
}
