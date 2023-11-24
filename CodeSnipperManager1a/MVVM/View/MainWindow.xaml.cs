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

        public async void PopulateGrid() 
        { 
            Task<List<Snippet>> snippetsTask = databaseAccess.GetSnippets();
            
            List<Snippet> snippets = await snippetsTask;

            for (int i = 0; i < snippets.Count; i++)
            {
                //.Text = snippets[i].CodeSnippet;

                // The base border
                Border border = new Border();
                border.Margin = new Thickness(16, 0, 0, 0);
                border.HorizontalAlignment = HorizontalAlignment.Left;
                border.Background = Brushes.White;
                border.Width = 330;
                border.Height = 360;

                Canvas canvas = new Canvas();

                Label labelTitle = new Label
                {
                    Content = snippets[i].Title,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Inter"),
                    FontSize = snippets[i].Title.Length > 10 ? 20 : 24
                };
                Canvas.SetLeft(labelTitle, 23);
                Canvas.SetTop(labelTitle, 24);
                canvas.Children.Add(labelTitle);

                Label labelProgram = new Label
                {
                    Content = snippets[i].ProgrammingLanguage,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Inter"),
                    FontSize = snippets[i].ProgrammingLanguage.Length <= 4 ? 24 : 20,
                };
                int CavasLeft = snippets[i].ProgrammingLanguage.Length <= 4 ? 243 : 211;
                Canvas.SetLeft(labelProgram, CavasLeft);
                Canvas.SetTop(labelProgram, 24);
                canvas.Children.Add(labelProgram);

                TextBlock textBlock = new TextBlock
                {
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Text = snippets[i].Description,
                    Width = 230,
                    Height = 45,
                    FontFamily = new FontFamily("Inter"),
                    FontSize = 15,
                    Foreground = new SolidColorBrush(Color.FromRgb(101, 101, 101))
                };
                Canvas.SetLeft(textBlock, 23);
                Canvas.SetTop(textBlock, 68);
                canvas.Children.Add(textBlock);

                TextBox textBoxCode = new TextBox
                {
                    Name = "textBoxCode",
                    Text = snippets[i].CodeSnippet,
                    FontSize = 13,
                    IsReadOnly = true,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(2),
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Height = 237,
                    Width = 310
                };
                Canvas.SetLeft(textBoxCode, 10);
                Canvas.SetTop(textBoxCode, 113);
                canvas.Children.Add(textBoxCode);

                border.Child = canvas;

                spDisplayPanel.Children.Add(border);
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
