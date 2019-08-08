using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Discord.Pages {
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page {
        private string emailFormat = "\\S+@\\S+\\.\\S+";
        private Regex regex;
        private MatchCollection matchCollection;
        private APICaller apiCaller;
        public SignUpPage() {
            InitializeComponent();
            regex = new Regex(emailFormat);
            apiCaller = new APICaller();
        }

        private bool ContainsUnicode(string text) {
            if (text.Any(c => c > 255)) {
                return true;
            }
            return false;
        }
        private async void ButtonSignUp_Click(object sender, RoutedEventArgs e) {
            matchCollection = regex.Matches(TextBoxEmail.Text);
            if(matchCollection.Count == 0) {
                MessageBox.Show("Invalid email format");
                return;
            }
            if (ContainsUnicode(TextBoxEmail.Text)) {
                MessageBox.Show("Email cannot contain unicode characters");
                return;
            }
            if(PasswordBox.Password == "") {
                MessageBox.Show("Password cannot be empty");
                return;
            }
            if (ContainsUnicode(PasswordBox.Password)) {
                MessageBox.Show("Password cannot contain unicode characters");
                return;
            }
            if (ContainsUnicode(PasswordBoxConfirm.Password)) {
                MessageBox.Show("Password confirm cannot contain unicode characters");
                return;
            }
            if (PasswordBox.Password != PasswordBoxConfirm.Password) {
                MessageBox.Show("Password confirmation does not match");
                return;
            }
            if (TextBoxUsername.Text == "") {
                MessageBox.Show("Username cannot be empty");
                return;
            }
            if (ContainsUnicode(TextBoxUsername.Text)) {
                MessageBox.Show("Username cannot contain unicode characters");
                return;
            }
            if (TextBoxFirstName.Text == "" && TextBoxLastName.Text == "") {
                MessageBox.Show("First name and last name cannot be both empty");
                return;
            }
            string email = TextBoxEmail.Text;
            string password = PasswordBox.Password;
            //string userName = TextBoxUsername.Text;
            //string firstName = TextBoxFirstName.Text;
            //string lastName = TextBoxLastName.Text;
            User user = new User {
                Email = TextBoxEmail.Text,
                Password = PasswordBox.Password,
                UserName = TextBoxUsername.Text,
                FirstName = TextBoxFirstName.Text,
                LastName = TextBoxLastName.Text,
                Gender = (bool) RadioButtonMale.IsChecked ? Gender.Male : Gender.Female
            };
            //string outgoingJson = JsonBuilder.BuildUserJson(email, password, userName, firstName, lastName, gender);
            string requestUrl = Route.User.UrlSignUp;
            apiCaller.SetProperties(HttpMethod.Post, requestUrl, user);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            if (httpResponseMessage.IsSuccessStatusCode) {
                string result = await httpResponseMessage.Content.ReadAsStringAsync();
                FileSystem.WriteUserData(email, password);
                Inventory.SetCurrentUser(result);
                Program.mainPage = new MainPage();
                await Program.mainWindow.BeginMainAsync();
            }
            else {
                MessageBox.Show("Something went wrong.");
            }
        }
    }
}
