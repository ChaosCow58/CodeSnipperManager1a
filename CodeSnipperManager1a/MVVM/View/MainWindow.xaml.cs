using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private AddSnippet addWindow;
        private UpdateSnippet updateWindow;
        private DeleteSnippet deleteWindow;

        private SnippetDatabaseAccess databaseAccess;

        public MainWindow()
        {
            InitializeComponent();
            databaseAccess = new SnippetDatabaseAccess();

            PopulateGrid();
        }

        private void PopulateGrid() 
        { 
            Task<List<Snippet>> snippets = databaseAccess.GetSnippets();

            for (int i = 0;i < snippets.Result.Count;i++) 
            { 
                Border border = new Border();
                border.Margin = new Thickness(16, 0, 0, 0);
                border.HorizontalAlignment = HorizontalAlignment.Left;
                border.Background = Brushes.White;
                border.Width = 330;
                border.Height = 360;
            }
        }

        private void Clear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = ToolBox.FindTextBox("SearchBox", tbSearchBox);

            if (textBox != null) 
            {
                textBox.Clear();
            }
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
