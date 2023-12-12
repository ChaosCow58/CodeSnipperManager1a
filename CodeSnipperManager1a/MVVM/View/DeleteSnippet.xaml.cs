using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;

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

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/icon.ico"));

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
            this.Close();
        }

        private void NoDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
