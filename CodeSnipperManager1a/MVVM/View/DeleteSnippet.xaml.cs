using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            mainWindow.RefreshData();

            this.Close();
        }

        private void NoDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
