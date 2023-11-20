using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace CodeSnipperManager1a
{

    public class Item 
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public string[]? extensions { get; set; }
    }

    /// <summary>
    /// Interaction logic for AddSnippet.xaml
    /// </summary>
    public partial class AddSnippet : Window
    {
        public AddSnippet()
        {
            InitializeComponent();
            GenerateComboBox();
            
        }

        private void AddToMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void GenerateComboBox()
        {
            string filePath = @"Jsons/programmingLangs.json";
  
            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);

                List<Item>? langs = JsonConvert.DeserializeObject<List<Item>>(text);

                if (langs != null)
                {
                    foreach (Item lang in langs)
                    {
                        if (lang.type == "programming" && lang.extensions != null && lang.extensions.Length > 0)
                        {

                            cbProgramLang.Items.Add($"{lang.name} ({lang.extensions[0]})");
                        }
                    }
                }
            }
            else
            {
                Debug.WriteLine($"File not found: {filePath}");
            }
        }
    }
}
