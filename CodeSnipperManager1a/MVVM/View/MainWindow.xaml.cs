using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using CodeSnipperManager1a.MVVM.View;
using CodeSnipperManager1a.MVVM.ViewModel;

#pragma warning disable CS8618

namespace CodeSnipperManager1a
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class TextToDocumentConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Your conversion logic here
            // Example: Convert value (assumed to be string) to TextDocument
            string? textValue = value as string;

            if (!string.IsNullOrEmpty(textValue))
            {
                TextDocument document = new TextDocument();
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
        private Profile profileWindow;
        private AddSnippet addWindow;
        private UpdateSnippet updateWindow;
        private DeleteSnippet deleteWindow;

        private SnippetDatabaseAccess databaseAccess;
        private SnippetsViewModel viewSnippetModel;

        private string? SnippetId = "";
        private DateTime SnippetDate;
        private string? LangName = "";
     
        public MainWindow()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/Icons/windowIcon.ico"));

            databaseAccess = new SnippetDatabaseAccess();
            viewSnippetModel = new SnippetsViewModel();

            Login loginWindow = new Login();
            loginWindow.Owner = null;
            loginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loginWindow.ShowDialog();

            if (Globals.UserId == "") 
            {
                Close();
            }

            DataContext = viewSnippetModel;
             
            Loaded += MainWindow_Loaded;

            PopulateGrid();
         
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetSyntaxDefinitionBasedOnExtension("a");
        }

        #region Populate Data
        private void FilterItems(string? searchText, List<Snippet> snippets)
        {

            DateTime dateStart = new DateTime();
            DateTime dateEnd = new DateTime();

            List<string?> currentFilters = new List<string?>();
            currentFilters.Add("");

            foreach (object? item in cmFilterMenu.Items)
            {
                if (item is MenuItem menuItem)
                {
                    if (menuItem.IsChecked == true) 
                    {
                        if (menuItem.Tag != null)
                        {
                            currentFilters.Add(menuItem.Tag.ToString());
                        }
                    }
                    else if (menuItem.IsChecked == false && currentFilters.Count > 1 && currentFilters.Contains(menuItem.Tag.ToString()))
                    {
                        currentFilters.Remove(menuItem.Tag.ToString());
                    }
                    if (menuItem.HasItems)
                    {
                        foreach (object? item2 in menuItem.Items)
                        {
                            if (item2 is MenuItem menuItem2) 
                            {
                                if (menuItem2.IsChecked == true)
                                {
                                    currentFilters.Add(menuItem2.Tag.ToString());
                                }
                                else if (menuItem2.IsChecked == false && currentFilters.Count > 1 && currentFilters.Contains(menuItem2.Tag.ToString()))
                                {
                                    currentFilters.Remove(menuItem2.Tag.ToString());
                                }
                            }
                        }
                    }
                }
            }


            foreach (string? filter in currentFilters)
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
                        dateStart = DateTime.Now.Date;
                        dateEnd = dateStart.AddDays(1);

                        snippets = snippets
                            .Where(s => s.CreatedAt >= dateStart && s.CreatedAt < dateEnd)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToList();
                        break;
                    case "yesterday":
                        dateStart = DateTime.Now.Date;
                        dateEnd = dateStart.AddDays(-1);

                        snippets = snippets
                            .Where(s => s.CreatedAt >= dateEnd && s.CreatedAt < dateStart)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToList();
                        break;
                    case "last30Days":
                        dateStart = DateTime.Now.Date.AddDays(-30);

                        snippets = snippets
                            .Where(s => s.CreatedAt >= dateStart && s.CreatedAt <= DateTime.Now.Date)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToList();
                        break;
                    case "thisMonth":
                        dateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        dateEnd = dateStart.AddMonths(1);

                        snippets = snippets
                            .Where(s => s.CreatedAt >= dateStart && s.CreatedAt < dateEnd)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToList();
                        break;         
                    case "lastMonth":
                        dateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        dateEnd = dateStart.AddMonths(-1);

                        snippets = snippets
                            .Where(s => s.CreatedAt >= dateEnd && s.CreatedAt < dateStart)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToList();
                        break;
                    case "thisYear":
                        dateStart = new DateTime(DateTime.Now.Year, 1, 1);
                        dateEnd = dateStart.AddYears(1);

                        snippets = snippets
                            .Where(s => s.CreatedAt >= dateStart && s.CreatedAt < dateEnd)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToList();
                        break;
                    case "lastYear":
                        dateStart = new DateTime(DateTime.Now.Year, 1, 1);
                        dateEnd = dateStart.AddYears(-1);

                        snippets = snippets
                            .Where(s => s.CreatedAt >= dateEnd && s.CreatedAt < dateStart)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToList();
                        break;
                    default:
                        snippets = snippets.OrderByDescending(s => s.CreatedAt).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(LangName))
            {
                snippets = snippets
                    .Where(s => s.ProgrammingLanguage.Contains(LangName))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                snippets = snippets
                    .Where(s => s.Title.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            viewSnippetModel.Items?.Clear();
            snippets.ForEach(snippet => viewSnippetModel.Items?.Add(snippet));
        }

        public async void PopulateGrid()
        {
            Task<List<Snippet>> snippetsTask = databaseAccess.GetSnippets();
            List<Snippet> snippets = await snippetsTask;

            TextBox? SearchTextBox = ToolBox.FindTextBox("SearchBox", tbSearchBox);

            snippets = snippets.Where(s => s.UserId == Globals.UserId).ToList();

            FilterItems(SearchTextBox?.Text, snippets);

            miProgramminLang.Items.Clear();

           IEnumerable<Snippet>? sortedItems = viewSnippetModel.Items
                .OrderBy(item => item.ProgrammingLanguage)
                .GroupBy(item => item.ProgrammingLanguage)
                .Select(group => group.First());

            MenuItem resetLang = new MenuItem
            {
                Header = "Reset Langauages",
                Tag = "",
                IsCheckable = false,

            };

            resetLang.Click += ResetLangs_MenuItem_Click;
            miProgramminLang.Items.Add(resetLang);

            Separator splash = new Separator();
            miProgramminLang.Items.Add(splash);

            foreach (Snippet snippet in sortedItems)
            {
               MenuItem menuItem = new MenuItem
                {
                    Header = snippet.ProgrammingLanguage,
                    Tag = snippet.ProgrammingLanguage,
                    IsCheckable = true,
                    
                };

                menuItem.Click += programmingLang_MenuItem_Click;

                miProgramminLang.Items.Add(menuItem);
            }

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
            using (Stream? stream = assembly.GetManifestResourceStream($"CodeSnipperManager1a.SyntaxShader.{xshdFileName}"))
            {
                if (stream == null)
                {
                    MessageBox.Show($"Syntax definition file {xshdFileName} not found.", "Error");
                    return;
                }

                using (XmlTextReader reader = new XmlTextReader(stream))
                {
                    XshdSyntaxDefinition xshd = HighlightingLoader.LoadXshd(reader);

                    // Create a new viewSnippetModel if needed
                    if (viewSnippetModel == null)
                    {
                        viewSnippetModel = new SnippetsViewModel();
                        icDataDisplay.DataContext = viewSnippetModel;
                    }

                    // Set SyntaxHighlighting using HighlightingLoader
                    viewSnippetModel.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
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
        private void Profile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            profileWindow = new Profile();

            profileWindow.Owner = this;
            profileWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            profileWindow.ShowDialog();
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

                foreach (object? child in ((Canvas)clickedBorder.Child).Children)
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

#pragma warning disable CS8603
        public string GetSnippetId() 
        {
            return SnippetId;
        }
#pragma warning restore CS8603


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

        public void ClearSelection()
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
            foreach (object? item in cmFilterMenu.Items)
            {
                if (item is MenuItem menuItem)
                {
                    if (menuItem.IsChecked == true)
                    {
                        menuItem.IsChecked = false;
                    }
                    if (menuItem.HasItems) 
                    {
                        foreach (object? item2 in menuItem.Items) 
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

            LangName = null;
            PopulateGrid();
        }

        #region Alphabetical Filters
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
            foreach (object? item in miDateMenu.Items)
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
            foreach (object? item in miDateMenu.Items)
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
            foreach (object? item in miDateMenu.Items)
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
            foreach (object? item in miDateMenu.Items)
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
            foreach (object? item in miDateMenu.Items)
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
            foreach (object? item in miDateMenu.Items)
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
            foreach (object? item in miDateMenu.Items)
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

        #region Programming Langauage Filters
        private void programmingLang_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem? menuItem1 = e.Source as MenuItem;

            foreach (object? item in miProgramminLang.Items)
            {
                if (item is MenuItem menuItem)
                {
                    if (menuItem.IsChecked == true && menuItem != sender)
                    {
                        menuItem.IsChecked = false;
                    }
                    if (menuItem.IsChecked == true && menuItem == sender)
                    {
                        LangName = menuItem.Tag.ToString();
                    }
                }

            }
            PopulateGrid();
        }

        private void ResetLangs_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LangName = null;
            PopulateGrid();
        }
        #endregion Programming Langauage Filters

        #endregion Filters
    }
}
