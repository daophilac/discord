using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DictionaryManagement.Pages {
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page {
        private DbSet<User> users;
        public DbSet<User> Users {
            get => users;
            set {
                users = value ?? throw new ArgumentNullException("Users cannot be null.");
            }
        }
        public LoginPage(DbSet<User> users) {
            InitializeComponent();
            Users = users;
        }
        private void CBLogin_Click(object sender, RoutedEventArgs e) {
            App.User = Users.Where(u => u.Email == CTEmail.Text && u.Password == CTPassword.Text).FirstOrDefault();
            if(App.User == null) {
                MessageBox.Show("Login failed.");
            }
            else {
                App.NavigateMain();
            }
        }
    }
}
