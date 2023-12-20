﻿using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.Model;
using CodeSnipperManager1a.MVVM.ViewModel;
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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {

        private UsersViewModel userViewModel;
        private SnippetDatabaseAccess databaseAccess;
        public Profile()
        {
            InitializeComponent();

            userViewModel = new UsersViewModel();
            databaseAccess = new SnippetDatabaseAccess();

            DataContext = userViewModel;
        }

        private async void SetImages() 
        { 
            Task<List<User>> userTask = databaseAccess.GetUser(Globals.UserId);
            List<User> userList = await userTask;

            foreach (User user in userList) 
            {
                imMenuProfile.Source = (ImageSource)FindResource(user.ProfileImage);
            }

            userList.ForEach(u => userViewModel.Items.Add(u));
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
    }
}