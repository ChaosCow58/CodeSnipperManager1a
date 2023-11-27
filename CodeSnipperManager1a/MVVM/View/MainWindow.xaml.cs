using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;
using CodeSnipperManager1a.MVVM.ModelView;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

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
        private SnippetsViewModel viewModel;

        private string? SnippetId = "";
        private DateTime SnippetDate;

        #pragma warning disable CS8618
        public MainWindow()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/icon.ico"));

            databaseAccess = new SnippetDatabaseAccess();
            viewModel = new SnippetsViewModel();

            DataContext = viewModel;
            PopulateGrid();
        }


        public void RefreshData()
        {
            PopulateGrid();

            icDataDisplay.UpdateLayout();
            icDataDisplay.InvalidateVisual();
            icDataDisplay.Items.Refresh();
        }

        public async void PopulateGrid()
        {
            Task<List<Snippet>> snippetsTask = databaseAccess.GetSnippets();
            List<Snippet> snippets = await snippetsTask;

            snippets = snippets.OrderByDescending(s => s.CreatedAt).ToList();

            viewModel.Items?.Clear(); // Clear existing items

            foreach (Snippet snippet in snippets)
            {
                viewModel.Items?.Add(snippet); // Add the new items
            }
        }


        private void Clear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = ToolBox.FindTextBox("SearchBox", tbSearchBox);
            textBox?.Clear();
        }

        private void Add_MouseUp(object sender, MouseButtonEventArgs e)
        {
            addWindow = new AddSnippet();

            addWindow.Owner = this;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addWindow.ShowDialog();

        }

        private void Update_MouseUp(object sender, MouseButtonEventArgs e)
        {
            updateWindow = new UpdateSnippet();

            updateWindow.Owner = this;
            updateWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            updateWindow.ShowDialog();

        }

        private void Delete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            deleteWindow = new DeleteSnippet();

            deleteWindow.Owner = this;
            deleteWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            deleteWindow.ShowDialog();
        }

        private void ClearSelection()
        {
            if (selectedBorder != null)
            {
                selectedBorder.Background = Brushes.White;
                selectedBorder = null;
                btUpdate.Visibility = Visibility.Hidden;
                btDelete.Visibility = Visibility.Hidden;
            }
        }


        private Border? selectedBorder;

        private void SelectBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border clickedBorder)
            {
               ClearSelection();

                // Select the clicked border
                clickedBorder.Background = new SolidColorBrush(Color.FromRgb(180, 208, 250));
                selectedBorder = clickedBorder;

                btUpdate.Visibility = Visibility.Visible;
                btDelete.Visibility = Visibility.Visible;

                string? label1 = "";
                DateTime label2 = new DateTime();

                foreach (var child in ((Canvas)clickedBorder.Child).Children)
                {
                    if (child is Label label)
                    {
                        if (label.Name.Equals("lBorderId"))
                        {
                            label1 = label.Content.ToString();
                        }
                        if (label.Name.Equals("lBorderDate")) 
                        { 
                            label2 = (DateTime)label.Content;
                        }

                    }
                }

                SnippetId = label1;
                SnippetDate = label2;

            }
        }

        public string? GetSnippetId() 
        {
            return SnippetId;
        }    
        
        public DateTime GetSnippetDate() 
        {
            return SnippetDate;
        }


        private void EnterBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            Border? border = e.Source as Border;
            if (border != null)
            {
                border.Effect = new DropShadowEffect 
                {
                    Color = Colors.LightBlue,
                    Direction = 320,
                    ShadowDepth = 5,
                    Opacity = 1,
                };
            }
        }

        private void LeaveBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            Border? border = e.Source as Border;
            if (border != null)
            {
                border.Effect = null;
            }
        }

        private void Container_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ClearSelection();
        }
    }
}
