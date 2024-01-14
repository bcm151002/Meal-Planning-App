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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tema.Database;

namespace Tema.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class RegisterView 
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void Windows_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            var borderBrushAnimation = FindResource("BorderBrushAnimation") as Storyboard;
            if (borderBrushAnimation != null)
            {
                borderBrushAnimation.Begin();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = txtUser.Text;
            string enteredPassword = txtPass.Password;
            string enteredEmail = txtEmail.Text;

            // Check if the username already exists
            dbDataContext context = new dbDataContext();
            var existingUser = (from u in context.Users
                                where u.username == enteredUsername
                                select u).SingleOrDefault();

            if (existingUser != null)
            {
                // Username already exists, show a message
                MessageBox.Show("Username already exists. Please choose a different username.");
            }
            else
            {
                // Username doesn't exist, add the new user to the database
                User newUser = new User
                {
                    username = enteredUsername,
                    password = enteredPassword,
                    email = enteredEmail,
                    role = "user" // Assuming "user" is the default role for new registrations
                                  // Add any other properties you have in your User class
                };

                context.Users.InsertOnSubmit(newUser);
                context.SubmitChanges();

                // Show a message indicating successful registration
                MessageBox.Show("Registration successful. You can now log in.");

                // Clear the registration fields if needed
                txtUser.Clear();
                txtPass.Clear();
                txtEmail.Clear();

                // Navigate back to the login page
                LoginView mw = new LoginView();
                mw.Show();
                this.Close();
            }
        }

    }
}
