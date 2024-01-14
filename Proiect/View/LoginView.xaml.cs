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
using Tema.Utilities;

namespace Tema.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = txtUser.Text;
            string enteredPassword = txtPass.Password;

            dbDataContext context = new dbDataContext();

            var user = (from u in context.Users
                        where u.username == enteredUsername && u.password == enteredPassword
                        select u).SingleOrDefault();

            if (user != null)
            {
                DataSharing.Instance.Username = enteredUsername;
                DataSharing.Instance.Context = context;
                // Authentication successful
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            else
            {
                // Authentication failed
                MessageBox.Show("Invalid username or password. Please try again.");
            }
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
            RegisterView mw = new RegisterView();
            mw.Show();
            this.Close();

        }
    }
}
