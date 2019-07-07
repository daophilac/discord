using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page {
        private string emailFormat = "\\S+@\\S+\\.\\S+";
        private Regex regex;
        private MatchCollection matchCollection;
        private APICaller apiCaller;
        private Gender gender;
        public SignUpPage() {
            InitializeComponent();
            regex = new Regex(emailFormat);
            apiCaller = new APICaller();
        }

        private async void ButtonSignUp_Click(object sender, RoutedEventArgs e) {
            matchCollection = regex.Matches(TextBoxEmail.Text);
            if(matchCollection.Count == 0) {
                MessageBox.Show("Invalid email format");
                return;
            }
            if(TextBoxPassword.Text == "") {
                MessageBox.Show("Password cannot be empty");
                return;
            }
            if (TextBoxPassword.Text != TextBoxConfirmPassword.Text) {
                MessageBox.Show("Password confirmation does not match");
                return;
            }
            if (TextBoxUserName.Text == "") {
                MessageBox.Show("User name cannot be empty");
                return;
            }
            if(TextBoxFirstName.Text == "" && TextBoxLastName.Text == "") {
                MessageBox.Show("First name and last name cannot be both empty");
                return;
            }
            if (RadioButtonMale.IsChecked == true) {
                gender = Gender.Male;
            }
            else {
                gender = Gender.Female;
            }
            string email = TextBoxEmail.Text;
            string password = TextBoxPassword.Text;
            string userName = TextBoxUserName.Text;
            string firstName = TextBoxFirstName.Text;
            string lastName = TextBoxLastName.Text;
            string outgoingJson = JsonBuilder.BuildUserJson(email, password, userName, firstName, lastName, gender);
            string requestUrl = Route.UrlSignUp;
            apiCaller.SetProperties(RequestMethod.POST, requestUrl, outgoingJson);
            string incomingJson = await apiCaller.SendRequestAsync();
            FileSystem.WriteUserData(email, password);
            Inventory.SetCurrentUser(incomingJson);
            Program.mainPage = new MainPage();
            Program.mainWindow.MainFrame.Navigate(Program.mainPage);
        }
    }
}
