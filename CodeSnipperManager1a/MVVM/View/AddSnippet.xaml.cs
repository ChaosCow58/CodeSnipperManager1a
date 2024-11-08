﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;

#pragma warning disable CS8618, CS8619

namespace CodeSnipperManager1a.MVVM.View
{
    /// <summary>
    /// Interaction logic for AddSnippet.xaml
    /// </summary>
    public partial class AddSnippet : Window
    {
        private Snippet SnippetModel;

        private SnippetDatabaseAccess databaseAccess;


        public AddSnippet()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/Icons/windowIcon.ico"));

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
                    UserId = Globals.UserId,
                    Description = DescriptionTextBox.Text == "" ? "No Description" : DescriptionTextBox.Text,
                    ProgrammingLanguage = ToolBox.ParseComboBox(cbProgramLang),
                    CodeSnippet = CodeSnippetBox.Text
                };

                databaseAccess.CreateSnippet(SnippetModel);

                // Get a reference to the main window
                MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

                // Call the PopulateGrid method in the main window
                mainWindow?.PopulateGrid();

                Close();
            }
        }

        private void Cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
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

