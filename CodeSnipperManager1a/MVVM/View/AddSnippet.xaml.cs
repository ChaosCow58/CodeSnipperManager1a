using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using CodeSnipperManager1a.Core;

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
            ToolBox.GenerateComboBox(cbProgramLang);
            
        }

        private void AddToMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
