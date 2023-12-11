using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Xml;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

﻿using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;
using CodeSnipperManager1a.MVVM.ModelView;
using System.Net.NetworkInformation;
using System.Diagnostics;


namespace CodeSnipperManager1a
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class TextToDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Your conversion logic here
            // Example: Convert value (assumed to be string) to TextDocument
            string textValue = value as string;
            if (!string.IsNullOrEmpty(textValue))
            {
                var document = new TextDocument();
                document.Text = textValue;
                return document;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Implement if necessary
            throw new NotImplementedException();
        }
    }

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

            Loaded += MainWindow_Loaded;

            PopulateGrid();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetSyntaxDefinitionBasedOnExtension("a");
        }

        #region Populate Data
        private void FilterItems(string searchText, List<Snippet> snippets)
        {
            List<string> currentFilters = new List<string>();
            currentFilters.Add("");

            foreach (var item in cmFilterMenu.Items)
            {
                if (item is MenuItem menuItem)
                {
                    if (menuItem.HasItems)
                    {
                        foreach (var item2 in menuItem.Items)
                        {
                            if (item2 is MenuItem menuItem2) 
                            {
                                currentFilters = ProcessSubMenuItems(menuItem2);
                            }
                            else if (menuItem.IsChecked == false && currentFilters.Count > 1 && currentFilters.Contains(menuItem.Tag.ToString()))
                            {
                                currentFilters.Add(menuItem.Tag.ToString());
                            }
                        }
                    }
                }
            }


            foreach (string filter in currentFilters)
            {
                Debug.WriteLine($"Filters: {filter}");

                switch (filter)
                {
                    case "aToZ":
                        snippets = snippets.OrderBy(s => s.Title).ToList();
                        break;
                    case "zToA":
                        snippets = snippets.OrderByDescending(s => s.Title).ToList();
                        break;
                    case "today":
                        DateTime todayStart = DateTime.Now.Date;
                        DateTime todayEnd = todayStart.AddDays(1).AddTicks(-1);

                        snippets = snippets
                            .Where(s => s.CreatedAt >= todayStart && s.CreatedAt <= todayEnd)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToList();
                        break;
                    default:
                        snippets = snippets.OrderByDescending(s => s.CreatedAt).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                snippets = snippets
                    .Where(s => s.Title.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            viewModel.Items?.Clear();
            snippets.ForEach(snippet => viewModel.Items?.Add(snippet));
        }

        private List<string> ProcessSubMenuItems(MenuItem menuItem)
        {
            List<string> result = new List<string>();
            foreach (var subItem in menuItem.Items)
            {
                if (subItem is MenuItem subMenuItem && subMenuItem.IsChecked == true && subMenuItem.Tag != null)
                {
                    result.Add(subMenuItem.Tag.ToString());
                }
            }
            return result;
        }

        public async void PopulateGrid()
        {
            Task<List<Snippet>> snippetsTask = databaseAccess.GetSnippets();
            List<Snippet> snippets = await snippetsTask;

            TextBox? SearchTextBox = ToolBox.FindTextBox("SearchBox", tbSearchBox);

            FilterItems(SearchTextBox?.Text, snippets);
            

            UpdateLayout();
        }
        #endregion Populate Data

        #region Syntax Highlighting
        private void SetSyntaxDefinitionBasedOnExtension(string fileExtension) 
        { 
            switch (fileExtension) 
            {
                default:
                    LoadSyntaxDefinition("SytnaxHighlight.xshd");
                    break;
            }
        }

        private void LoadSyntaxDefinition(string xshdFileName)
        {
            Assembly assembly = typeof(MainWindow).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream($"CodeSnipperManager1a.SyntaxShader.{xshdFileName}"))
            {
                if (stream == null)
                {
                    MessageBox.Show($"Syntax definition file {xshdFileName} not found.", "Error");
                    return;
                }

                using (var reader = new XmlTextReader(stream))
                {
                    var xshd = HighlightingLoader.LoadXshd(reader);

                    // Create a new ViewModel if needed
                    if (viewModel == null)
                    {
                        viewModel = new SnippetsViewModel();
                        icDataDisplay.DataContext = viewModel;
                    }

                    // Set SyntaxHighlighting using HighlightingLoader
                    viewModel.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
                }
            }
        }
        #endregion Syntax Highlighting

        #region Top Bar

        private void Clear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox SearchBox = ToolBox.FindTextBox("SearchBox", tbSearchBox);
            SearchBox?.Clear();
            PopulateGrid();
            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
        }
        #endregion Top Bar

        #region Window Calls
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
        #endregion Window Calls

        #region Border Selection
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

        private void Container_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ClearSelection();
        }
        #endregion Border Selection

        #region Filters
        private void Filter_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image clickedImage)
            {
                // Check if the Image has a ContextMenu
                if (clickedImage.ContextMenu != null)
                {
                    // Show the context menu
                    clickedImage.ContextMenu.IsOpen = true;
                }
            }
        }
        private void Filter_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Image clickedImage)
            {
                // Check if the Image has a ContextMenu
                if (clickedImage.ContextMenu != null && clickedImage.ContextMenu.IsOpen == false)
                {
                    // Show the context menu
                    clickedImage.ContextMenu.IsOpen = true;
                }
            }
        }

        private void ClearFilters_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in cmFilterMenu.Items)
            {
                if (item is MenuItem menuItem)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                    if (menuItem.HasItems) 
                    {
                        foreach (var item2 in menuItem.Items) 
                        {
                            if (item2 is MenuItem menuItem2)
                            {
                                if (menuItem2.IsChecked == true)
                                {
                                    menuItem2.IsChecked = false;
                                }
                            }
                        }
                    }
                }
            }
            PopulateGrid();
        }

        #region Alphabetical FIlters
        private void SortAZ_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (miZA.IsChecked == true) 
            { 
                miZA.IsChecked = false;
            }
           PopulateGrid();
        }

        private void SortZA_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (miAZ.IsChecked == true) 
            { 
                miAZ.IsChecked = false;
            }
            PopulateGrid();
        }
        #endregion Alphabetical FIlters

        #region Data Filters
        private void today_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in miDateMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem != sender)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
            PopulateGrid();
        }

        private void yesterday_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in miDateMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem != sender)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
            PopulateGrid();
        }

        private void last30Days_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in miDateMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem != sender)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
            PopulateGrid();
        }

        private void thisMonth_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in miDateMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem != sender)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
            PopulateGrid();
        }

        private void lastMonth_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in miDateMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem != sender)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
            PopulateGrid();
        }

        private void thisYear_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in miDateMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem != sender)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
            PopulateGrid();
        }

        private void lastYear_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in miDateMenu.Items)
            {
                if (item is MenuItem menuItem && menuItem != sender)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
            PopulateGrid();
        }
        #endregion Date Filters
        #endregion Filters
    }
}
