﻿using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Collections.Generic;

using CodeSnipperManager1a.Core;
using CodeSnipperManager1a.MVVM.ViewModel;
using CodeSnipperManager1a.MVVM.Model;


namespace CodeSnipperManager1a.MVVM.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private SnippetDatabaseAccess databaseAccess;
        private UsersViewModel usersViewModel;

        public Login()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri("pack://application:,,,../../Assets/Icons/windowIcon.ico"));

            databaseAccess = new SnippetDatabaseAccess();
            usersViewModel = new UsersViewModel();

            IniFile iniFile = new IniFile("CodeSnippetManager.ini");
            if (iniFile.GetValue("SECURITY", "USERNAMELOGIN") != "")
            {
                cbSave.IsChecked = true;
                tbusername.Text = iniFile.GetValue("SECURITY", "USERNAMELOGIN");
                pbpassword.Focus();
            }
            else
            {
                tbusername.Focus();
            }
        }

        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            Globals.UserId = "";
            Close();
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            Task<List<User>> usersTask = databaseAccess.GetUsers();
            List<User> users = await usersTask;

            string encryptedPassword = ToolBox.Encrypt(pbpassword.Password);
            IniFile iniFile = new IniFile("CodeSnippetManager.ini");

            foreach (User user in users)
            {
                if (tbusername.Text == user.Username && encryptedPassword == user.Password) 
                { 
                    Globals.UserId = user.Id;

                    if (cbSave.IsChecked == true)
                    {
                        iniFile.SetValue("SECURITY", "USERNAMELOGIN", tbusername.Text);
                        iniFile.Save();
                    }
                    else 
                    {
                        iniFile.SetValue("SECURITY", "USERNAMELOGIN", "");
                        iniFile.Save();
                    }
                    Close();
                    break;
                }
            }
            if (Globals.UserId == "") 
            {
                MessageBox.Show("Invalid Username or Password");
            }
        }

        private void SignUp_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SignUp signUpWindow = new SignUp();
            signUpWindow.Owner = null;
            signUpWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            signUpWindow.ShowDialog();
        }
    }
}