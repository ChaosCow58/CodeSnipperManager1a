using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;

#pragma warning disable CS8622

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

            StateChanged += Login_StateChanged;

        }

        private void Login_StateChanged(object sender, EventArgs e)
        {
            // Check if the new state is Minimized, and prevent it by setting it back to Normal
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
        }

        private void Login_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            Globals.UserId = "";
            App.Current.Shutdown();
        }

        private async void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Task<List<User>> usersTask = databaseAccess.GetUsers();
            List<User> users = await usersTask;
            string encryptedPassword = ToolBox.Encrypt(pbpassword.Password);

            foreach (User user in users) 
            {
                if (user.Username == tbusername.Text) 
                {
                    MessageBox.Show("That Username already exists!");
                    return;
                }
            }

            User NewUser = new User
            { 
                Username = tbusername.Text,
                Password = encryptedPassword,
                ProfileImage = "pack://siteoforigin:,,,/Profile/defaultProfile.png",
                Email = tbemail.Text
            };

            databaseAccess?.CreateUser(NewUser);
            Close();
        }
    }
}
