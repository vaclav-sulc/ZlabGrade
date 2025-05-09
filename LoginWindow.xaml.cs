﻿using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace Zaverecny_Projekt
{
    public partial class LoginWindow : Window
    {
        /*                  TO DO LIST
         * 
         *  Upravit hover effect u tlačítka "Login_Button"
         *  Přidat label varování zvlášt pro login a heslo
         *  Šifrovat hesla v databázi
         * 
         */

        public LoginWindow()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------\\
        //                          Input Field Text Effects                         \\
        //---------------------------------------------------------------------------\\

        readonly string loginText = "Uživatelské jméno";
        readonly string passwordText = "Heslo";
        readonly int fontSize_focused = 20;
        readonly int fontSize_unfocused = 16;
        readonly SolidColorBrush color_unfocused = Brushes.Gray;
        readonly SolidColorBrush color_focused = Brushes.Black;

        private void Login_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Login_TextBox.Text == loginText)
            {
                ModifyLoginTextVisuals(true);
            }
        }

        private void Login_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Login_TextBox.Text == string.Empty)
            {
                ModifyLoginTextVisuals(false);
            }
        }

        private void Password_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password_TextBox.Text == passwordText)
            {
                ModifyPasswordTextVisuals(true);
            }
        }

        private void Password_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password_TextBox.Text == string.Empty)
            {
                ModifyPasswordTextVisuals(false);
            }
        }

        private void ModifyLoginTextVisuals(bool focused)
        {
            if (focused)
            {
                Login_TextBox.Text = string.Empty;

                Login_TextBox.FontSize = fontSize_focused;
                Login_TextBox.Foreground = color_focused;
                Login_TextBox.FontStyle = FontStyles.Normal;
            }
            else
            {
                Login_TextBox.Text = loginText;

                Login_TextBox.FontSize = fontSize_unfocused;
                Login_TextBox.Foreground = color_unfocused;
                Login_TextBox.FontStyle = FontStyles.Italic;
            }
        }

        private void ModifyPasswordTextVisuals(bool focused)
        {
            if (focused)
            {
                Password_TextBox.Text = string.Empty;

                Password_TextBox.FontSize = fontSize_focused;
                Password_TextBox.Foreground = color_focused;
                Password_TextBox.FontStyle = FontStyles.Normal;
            }
            else
            {
                Password_TextBox.Text = passwordText;

                Password_TextBox.FontSize = fontSize_unfocused;
                Password_TextBox.Foreground = color_unfocused;
                Password_TextBox.FontStyle = FontStyles.Italic;
            }
        }

        //---------------------------------------------------------------------------\\
        //                          User Authentication                              \\
        //---------------------------------------------------------------------------\\

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new("server=sql7.freesqldatabase.com;user=sql7776236;password=rakYbIVDef;database=sql7776236;");
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT * FROM zlabgrade WHERE login = \"{Login_TextBox.Text.ToLower()}\" AND heslo = \"{Password_TextBox.Text}\"";
                MySqlCommand command = new(sqlQuery, mySqlConnection);
                
                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    Warning_Label.Visibility = Visibility.Hidden;

                    switch (dataReader["role"])
                    {
                        case "vedeni":
                            break;

                        case "ucitel":

                            ucitelView ucitelView = new();
                            this.Close();
                            ucitelView.Show();
                            break;

                        case "student":

                            Zak_vzhled studentWindow = new();
                            this.Close();
                            studentWindow.Show();
                            break;
                    }
                }
                else
                {
                    Warning_Label.Visibility = Visibility.Visible;

                    ModifyLoginTextVisuals(false);
                    ModifyPasswordTextVisuals(false);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }
        }
    }
}