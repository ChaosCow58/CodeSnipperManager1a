using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;
using CodeSnipperManager1a.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;
using Microsoft.Win32;

namespace CodeSnipperManager1a.MVVM.View
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {

        private UsersViewModel userViewModel;
        private SnippetDatabaseAccess databaseAccess;
        public Profile()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/Icons/windowIcon.ico"));

            userViewModel = new UsersViewModel();
            databaseAccess = new SnippetDatabaseAccess();


            DataContext = userViewModel;


            SetImages();
        }

        private async void SetImages() 
        { 
            Task<List<User>> userTask = databaseAccess.GetUser(Globals.UserId);
            List<User> userList = await userTask;

            foreach (User user in userList) 
            {
                userList.ForEach(user => userViewModel.Items.Add(user));
            }


        }

        private void Bottom_MouseEnter(object sender, MouseEventArgs e)
        {
            Border border = (Border)e.Source;

            border.Background = Brushes.DarkGray;
        }

        private void Bottom_MouseLeave(object sender, MouseEventArgs e)
        {
            Border border = (Border)e.Source;

            border.Background = Brushes.Transparent;
        }

        private void LogOff_MouseUp(object sender, MouseButtonEventArgs e)
        {
           Close();
           Login loginWindow = new Login();
           loginWindow.Owner = null;
           loginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           loginWindow.ShowDialog();

           if (Globals.UserId == "") 
           {
               App.Current.Shutdown();
           }
        }

        private void ExitApp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void ExitProfile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private async void EditProfile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string userDowloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"Downloads\");

            OpenFileDialog fileDialog = new OpenFileDialog 
            {
                Title = "Select a Profile Image",
                InitialDirectory = userDowloadPath,
                Filter = "Image Files (*.png, *.jpeg, *.jpg, *.ico, *.gif, *.tiff, *bmp) | *.png;*.jpeg;*.jpg;*.ico;*.gif;*.tiff;*.bmp"
            };

            bool? result = fileDialog.ShowDialog();

            string profileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profile", Path.GetFileName(fileDialog.FileName));

            Task<List<User>> userTask = databaseAccess.GetUser(Globals.UserId);
            List<User> userList = await userTask;

            if (result == true) 
            {
                File.Copy(fileDialog.FileName, profileDirectory, true);

                foreach (User user in userList)
                {
                    User updateUser = new User
                    {
                        Id = Globals.UserId,
                        Username = user.Username,
                        Email = user.Email,
                        ProfileImage = "pack://siteoforigin:,,,/Profile/" + Path.GetFileName(fileDialog.FileName),
                        Password = user.Password
                    };

                    await databaseAccess.UpdateUser(updateUser);
                }

                userViewModel.Items.Clear();

                SetImages();
                UpdateLayout();
            }
        }

        private void Edit_MouseLeave(object sender, MouseEventArgs e)
        {
            Border border = (Border)e.Source;

            border.Background = Brushes.LightGray;
        }

        private void DeleteAccount_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult confirmantion = MessageBox.Show("Are you sure you want to DELETE your account?");

            if (confirmantion == MessageBoxResult.Yes) 
            {
                MessageBoxResult confirmantionTwo = MessageBox.Show("Are you really sure you want to DELETE your account?");

                if (confirmantionTwo == MessageBoxResult.Yes) 
                {
                    User deleteUser = new User
                    {
                        Id = Globals.UserId,
                    };

                    databaseAccess.DeleteUser(deleteUser);
                }
            }
        }
    }
}
