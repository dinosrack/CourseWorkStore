using CourseWorkStore.MessageBoxes;
using CourseWorkStore.Models;
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

namespace CourseWorkStore
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void ForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetPassword resetpassWindow = new ResetPassword();
            resetpassWindow.Owner = this;
            resetpassWindow.ShowDialog();
        }

        private void DemoVersion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserName.Text = "Пользователь";
            Password.Password = "1234567890"; 

            PerformLogin();
        }

        private void PerformLogin()
        {
            string username = UserName.Text; 
            string password = Password.Password;

            using (CourseWorkStoreContext _db = new CourseWorkStoreContext())
            {
                var user = _db.Users.FirstOrDefault(u => u.Username == username && u.Pass == password);

                if (user != null) 
                {
                    MainWindow mainWindow = new MainWindow(user.Username);
                    this.Close();
                    mainWindow.ShowDialog();
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserName.Text; 
            string password = Password.Password; 

            using (CourseWorkStoreContext _db = new CourseWorkStoreContext())
            {
                var user = _db.Users.FirstOrDefault(u => u.Username == username && u.Pass == password);

                if (user != null) 
                {
                    MainWindow mainWindow = new MainWindow();
                    this.Close();
                    mainWindow.ShowDialog();
                }
                else 
                {
                    InvalidAuth invalidAuth = new InvalidAuth();
                    invalidAuth.Owner = this;
                    invalidAuth.ShowDialog();
                }
            }
        }
    }
}
