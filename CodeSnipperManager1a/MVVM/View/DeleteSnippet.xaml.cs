using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;

#pragma warning disable CS8602

namespace CodeSnipperManager1a
{
    /// <summary>
    /// Interaction logic for DeleteSnippet.xaml
    /// </summary>
    public partial class DeleteSnippet : Window
    {

        private SnippetDatabaseAccess databaseAccess;
        public DeleteSnippet()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/Icons/windowIcon.ico"));

            databaseAccess = new SnippetDatabaseAccess();
        }

        private void YesDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {

            MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

            Snippet deleteSnippet = new Snippet()
            {
                Id = mainWindow.GetSnippetId()
            };

            databaseAccess.DeleteSnippet(deleteSnippet);
            mainWindow.PopulateGrid();
            mainWindow.ClearSelection();

            Close();
        }

        private void NoDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
