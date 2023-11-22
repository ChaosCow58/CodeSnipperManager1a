using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CodeSnipperManager1a.Core
{
    public class Item
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public string[]? extensions { get; set; }
    }

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
        
        public static List<Item> GetJson()
        {
            string filePath = @"CodeSnipperManager1a.Jsons.programmingLangs.json";
            string jsonContent = ReadEmbeddedResource(filePath);


            List<Item>? langs = JsonConvert.DeserializeObject<List<Item>>(jsonContent);

            return langs;
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

        public static TextBox FindTextBox(string name, DependencyObject container)
        {
            if (container == null) return null;

            var childCount = VisualTreeHelper.GetChildrenCount(container);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(container, i);

                if (child is TextBox textBox && textBox.Name == name)
                {
                    return textBox;
                }

                var result = FindTextBox(name, child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
