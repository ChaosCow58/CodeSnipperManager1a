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

namespace CodeSnipperManager1a.MVVM.View
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {

        private SnippetDatabaseAccess databaseAccess; 

        public SignUp()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/Icons/windowIcon.ico"));

            databaseAccess = new SnippetDatabaseAccess();

        }

        private void Login_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            Globals.UserId = "";
            Close();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            string encryptedPassword = ToolBox.Encrypt(pbpassword.Password); 

            User user = new User
            { 
                Username = tbusername.Text,
                Password = encryptedPassword,
                Email = tbemail.Text
            };

            databaseAccess.CreateUser(user);
            Close();
        }

  
    }
}
