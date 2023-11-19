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
        public string Name { get; set; }
        public string Type { get; set; }
        public string[] Extensions { get; set; }
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

            string text = File.ReadAllText("Jsons/programmingLangs.json");
            Console.WriteLine(text);
            var langs = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(text);
            foreach (var lang in langs)
            {
                Console.WriteLine("*" + lang == "" ? "is null" : lang);
            }
        }


    }
}
