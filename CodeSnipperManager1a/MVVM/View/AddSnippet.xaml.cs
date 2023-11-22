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
using CodeSnipperManager1a.MVVM.Model;
using MongoDB.Driver.Core.Misc;

namespace CodeSnipperManager1a
{
    /// <summary>
    /// Interaction logic for AddSnippet.xaml
    /// </summary>
    public partial class AddSnippet : Window
    {
        private Snippet SnippetModel;

        private TextBox TitleTextBox;
        private TextBox DescriptionTextBox;
        private TextBox CodeSnippetBox;

        private SnippetDatabaseAccess databaseAccess;


        public AddSnippet()
        {
            InitializeComponent();
            ToolBox.GenerateComboBox(cbProgramLang);

            databaseAccess = new SnippetDatabaseAccess();

            
        }

        private void AddToMain_MouseUp(object sender, MouseButtonEventArgs e)
        {

            TextBox TitleTextBox = ToolBox.FindTextBox("TitleBox", tbTitleBox);
            TextBox DescriptionTextBox = ToolBox.FindTextBox("DescriptionBox", tbDescriptionBox);
            TextBox CodeSnippetBox = ToolBox.FindTextBox("SnippetBox", tbCodeSnippet);

            if (CheckTextBoxes())
            {
                SnippetModel = new Snippet()
                {
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextBox.Text == "" ? "No Description" : DescriptionTextBox.Text,
                    ProgrammingLanguage = ParseComboBox(),
                    CodeSnippet = CodeSnippetBox.Text
                };

                databaseAccess.CreateSnippet(SnippetModel);

                this.Close();
            }
        }

        private void Cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private string ParseComboBox() 
        {
            List<Item> items = ToolBox.GetJson();
            string name = "";

            if (items != null)
            {
                foreach (Item item in items)
                {
                    if (item.extensions != null && item.extensions.Length > 0)
                    {                      
                        if (cbProgramLang.SelectedItem.ToString() == $"{item.name} ({item.extensions[0]})") 
                        {
                            name += cbProgramLang.SelectedItem.ToString().Substring(0, item.name.Length);
                            break;
                        }
                    }
                }
            }

            return name;
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
