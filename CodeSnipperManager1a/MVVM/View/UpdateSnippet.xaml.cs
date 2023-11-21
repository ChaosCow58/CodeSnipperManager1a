using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CodeSnipperManager1a.Core;

namespace CodeSnipperManager1a
{
    /// <summary>
    /// Interaction logic for UpdateSnippet.xaml
    /// </summary>
    public partial class UpdateSnippet : Window
    {

        public UpdateSnippet()
        {
            InitializeComponent();
            ToolBox.GenerateComboBox(cbProgramLang);
        }

        private void SaveToMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
