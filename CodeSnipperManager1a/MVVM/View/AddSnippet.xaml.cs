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


        public AddSnippet()
        {
            InitializeComponent();
            ToolBox.GenerateComboBox(cbProgramLang);

            TitleTextBox = ToolBox.FindTextBox("TitleBox", tbTitleBox);
            DescriptionTextBox = ToolBox.FindTextBox("DescriptionBox", tbDescriptionBox);
            CodeSnippetBox = ToolBox.FindTextBox("SnippetBox", tbCodeSnippet);
        }

        private void AddToMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CheckTextBoxes())
            {
                Debug.WriteLine(ParseComboBox());
             /*   SnippetModel = new Snippet()
                {
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextBox.Text == "" ? "No Description" : DescriptionTextBox.Text,
                    
                };*/


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

            if (cbProgramLang.SelectedValue != null) 
            {
                for (int i = 0; i < items.Count; i++)
                {

                    if (cbProgramLang.SelectedValue.ToString() == $"{items[i].name} ({items[i].extensions[0]})")
                    {
                        name += cbProgramLang.SelectedValue.ToString().Substring(0, items[i].extensions[0].Length);
                        break;
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
