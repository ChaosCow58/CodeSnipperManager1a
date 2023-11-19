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
        public MainWindow()
        {
            InitializeComponent();
 /*           var resourceDictionary = new ResourceDictionary
            {
                Source = new System.Uri("/CodeSnipperManager1a;component/Resources/Searchbox.xaml", System.UriKind.RelativeOrAbsolute)
            };
            serachBoxStyle = resourceDictionary["SearchBox"] as Style;*/

        }

        private void Clear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            
        }

        private void Add_MouseUp(object sender, MouseButtonEventArgs e)
        {
            addWindow = new AddSnippet();

            addWindow.Owner = null;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addWindow.ShowDialog();

        }
    }
}
