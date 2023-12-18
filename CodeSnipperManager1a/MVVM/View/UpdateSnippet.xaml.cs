using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;

namespace CodeSnipperManager1a
{
    /// <summary>
    /// Interaction logic for UpdateSnippet.xaml
    /// </summary>
    public partial class UpdateSnippet : Window
    {

        private SnippetDatabaseAccess databaseAccess;


        public UpdateSnippet()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/Icons/windowIcon.ico"));

            Loaded += UpdateSnippet_Loaded;

            ToolBox.GenerateComboBox(cbProgramLang);

            databaseAccess = new SnippetDatabaseAccess();
        }

        private void UpdateSnippet_Loaded(object sender, RoutedEventArgs e)
        {
            GetSnippetInfo();
        }

        private void SaveToMain_MouseUp(object sender, MouseButtonEventArgs e)
        {

            TextBox TitleTextBox = ToolBox.FindTextBox("TitleBox", tbTitleBox);
            TextBox DescriptionTextBox = ToolBox.FindTextBox("DescriptionBox", tbDescriptionBox);
            TextBox CodeSnippetBox = ToolBox.FindTextBox("SnippetBox", tbCodeSnippet);

            if (CheckTextBoxes()) 
            {
                MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

                Snippet updatedSnippet = new Snippet
                {
                    Id = mainWindow.GetSnippetId(),
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextBox.Text == "" ? "No Description" : DescriptionTextBox.Text,
                    CodeSnippet = CodeSnippetBox.Text,
                    ProgrammingLanguage = ToolBox.ParseComboBox(cbProgramLang),
                    CreatedAt = mainWindow.GetSnippetDate()
                };


                databaseAccess.UpdateSnippet(updatedSnippet);
                mainWindow.PopulateGrid();
                this.Close();
            }
        }

        private async void GetSnippetInfo()
        {
            TextBox TitleTextBox = ToolBox.FindTextBox("TitleBox", tbTitleBox);
            TextBox DescriptionTextBox = ToolBox.FindTextBox("DescriptionBox", tbDescriptionBox);
            TextBox CodeSnippetBox = ToolBox.FindTextBox("SnippetBox", tbCodeSnippet);

            MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

            Task<List<Snippet>> snippet = databaseAccess.GetSnippet(mainWindow.GetSnippetId());
            List<Snippet> snippetInfo = await snippet;

            foreach (Snippet snippet1 in snippetInfo)
            {
                TitleTextBox.Text = snippet1.Title;
                DescriptionTextBox.Text = snippet1.Description;
                CodeSnippetBox.Text = snippet1.CodeSnippet;

                for (int i = 0;i < cbProgramLang.Items.Count;i++)
                {
                    if (cbProgramLang.Items[i].ToString().Contains(snippet1.ProgrammingLanguage)) 
                    {
                        cbProgramLang.SelectedItem = cbProgramLang.Items[i];
                        break;
                    }
                }

            }
        }

        private void Cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private bool CheckTextBoxes()
        {
            bool TitleBoxError = false;
            bool CodeSnippetError = false;
            bool ProgrammingLangError = false;

            TextBox TitleTextBox = ToolBox.FindTextBox("TitleBox", tbTitleBox);
            TextBox CodeSnippetBox = ToolBox.FindTextBox("SnippetBox", tbCodeSnippet);

            if (TitleTextBox.Text == "")
            {
                TitleBoxError = true;
                bTitle.BorderBrush = Brushes.Red;
            }
            if (cbProgramLang.SelectedValue == null)
            {
                ProgrammingLangError = true;
                bProgramLang.BorderBrush = Brushes.Red;
            }
            if (CodeSnippetBox.Text == "")
            {
                CodeSnippetError = true;
                bCodeSnippet.BorderBrush = Brushes.Red;
            }

            if (TitleBoxError && CodeSnippetError && ProgrammingLangError)
            {
                MessageBox.Show("Please enter a Title, a Snippet, and select a Programming Langauge!");
                return false;
            }
            else if (TitleBoxError && CodeSnippetError)
            {
                MessageBox.Show("Please enter a Title and a Snippet!");
                return false;
            }
            else if (TitleBoxError && ProgrammingLangError)
            {
                MessageBox.Show("Please enter a Title and select a Programming Langauge!");
                return false;
            }
            else if (ProgrammingLangError && CodeSnippetError)
            {
                MessageBox.Show("Please enter a Snippet and select a Programming Langauge!");
                return false;
            }
            else if (TitleBoxError)
            {
                MessageBox.Show("Please enter a Title!");
                return false;
            }
            else if (CodeSnippetError)
            {
                MessageBox.Show("Please enter a Snippet!");
                return false;
            }
            else if (ProgrammingLangError)
            {
                MessageBox.Show("Please select a Programming Langauge!");
                return false;
            }

            return true;
        }
    }
}
