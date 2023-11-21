using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CodeSnipperManager1a
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Style serachBoxStyle;
        private AddSnippet addWindow;
        private UpdateSnippet updateWindow;
        private DeleteSnippet deleteWindow;

        private ResourceDictionary resourceDictionary;
   /*     private Dictionary<string, ControlTemplate> collection
        {
            get 
            { 
                Dictionary<string, ControlTemplate> contorls = new Dictionary<string, ControlTemplate>();
                contorls.Add("SearchTemplate", FindName("SearchTemplate") as ControlTemplate);
                return contorls;
            }
        }*/


        public MainWindow()
        {
            InitializeComponent();
            resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new System.Uri(@"Themes/SearchBox.xaml", System.UriKind.Relative);
        }

        private void Clear_MouseDown(object sender, MouseButtonEventArgs e)
        {
         /*   ControlTemplate? c = resourceDictionary[""] as ControlTemplate;
           
            Debug.WriteLine(c);*/
        }

        private void Add_MouseUp(object sender, MouseButtonEventArgs e)
        {
            addWindow = new AddSnippet();

            addWindow.Owner = null;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addWindow.ShowDialog();

        }

        private void Update_MouseUp(object sender, MouseButtonEventArgs e)
        {
            updateWindow = new UpdateSnippet();

            updateWindow.Owner = null;
            updateWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            updateWindow.ShowDialog();
        }

        private void Delete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            deleteWindow = new DeleteSnippet();

            deleteWindow.Owner = null;
            deleteWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            deleteWindow.ShowDialog();
        }
    }
}
