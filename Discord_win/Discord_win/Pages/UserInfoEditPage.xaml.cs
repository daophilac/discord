using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using Discord_win.ViewModels;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for UserInfoEditPage.xaml
    /// </summary>
    public partial class UserInfoEditPage : Page {
        public event EventHandler CancelEdit;
        public event EventHandler DoneEdit;
        private APICaller apiCaller;
        public string newUserImageFileName;
        public UserInfoEditPage() {
            InitializeComponent();
            apiCaller = new APICaller();
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            CancelEdit?.Invoke(this, EventArgs.Empty);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            TextBoxUserName.Text = Inventory.CurrentUser.Username;
            TextBoxEmail.Text = Inventory.CurrentUser.Email;
            if (Inventory.CurrentUser.ImageName != null) {
                UserImage.Source = await ImageResolver.DownloadBitmapImageAsync(Inventory.CurrentUser.ImageName);
            }
        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e) {
            if (TextBoxUserName.Text == "") {
                MessageBox.Show("User name cannot be empty.");
                return;
            }
            if (TextBoxEmail.Text == "") {
                MessageBox.Show("Email cannot be empty.");
                return;
            }
            if (PasswordBox.Password == "") {
                MessageBox.Show("Password cannot be empty.");
                return;
            }
            if(!await ConfirmPasswordAsync()) {
                MessageBox.Show("Incorrect password.");
                return;
            }
            if(!await CheckUnavailableEmailAsync()) {
                MessageBox.Show("Email Unavailable.");
                return;
            }
            UserUpdateProfileVM userUpdateProfileVM = new UserUpdateProfileVM {
                UserId = Inventory.CurrentUser.UserId,
                Username = TextBoxUserName.Text,
                Email = TextBoxEmail.Text
            };
            apiCaller.SetProperties(HttpMethod.Post, Route.UrlUpdateProfile, userUpdateProfileVM);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            if (httpResponseMessage.IsSuccessStatusCode) {
                Inventory.SetCurrentUser(await httpResponseMessage.Content.ReadAsStringAsync());
                if(newUserImageFileName != null) {
                    FileSystem.DeleteUserImage(Inventory.CurrentUser.ImageName);
                    ImageUploader imageUploader = new ImageUploader();
                    imageUploader.UploadUserImage(Inventory.CurrentUser.UserId, newUserImageFileName);
                }
                DoneEdit?.Invoke(this, EventArgs.Empty);
            }
            else {
                MessageBox.Show("Something went wrong.");
            }
        }
        private async Task<bool> ConfirmPasswordAsync() {
            UserConfirmPasswordVM userConfirmPasswordVM = new UserConfirmPasswordVM {
                UserId = Inventory.CurrentUser.UserId,
                Password = PasswordBox.Password
            };
            apiCaller.SetProperties(HttpMethod.Post, Route.UrlConfirmPassword, userConfirmPasswordVM);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            if (httpResponseMessage.IsSuccessStatusCode) {
                return true;
            }
            return false;
        }
        private async Task<bool> CheckUnavailableEmailAsync() {
            if(Inventory.CurrentUser.Email == TextBoxEmail.Text) {
                return true;
            }
            apiCaller.SetProperties(HttpMethod.Get, Route.BuildCheckUnavailableEmail(TextBoxEmail.Text));
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            if (httpResponseMessage.IsSuccessStatusCode) {
                return true;
            }
            return false;
        }
        private void ButtonCancel_MouseEnter(object sender, MouseEventArgs e) {
            ButtonCancel.Foreground = Brushes.Black;
        }
        private void ButtonCancel_MouseLeave(object sender, MouseEventArgs e) {
            ButtonCancel.Foreground = Brushes.White;
        }
        private void ButtonUpload_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp) | *.png;*.jpg;*.jpeg;*.bmp";
            openFileDialog.Multiselect = false;
            if(openFileDialog.ShowDialog() == true) {
                BitmapImage bitmapImage = ImageResolver.GetBitmapFromLocalFile(openFileDialog.FileName);
                if(bitmapImage == null) {
                    MessageBox.Show("File doesn't exist or broken.");
                    return;
                }
                UserImage.Source = bitmapImage;
                newUserImageFileName = openFileDialog.FileName;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            newUserImageFileName = null;
        }
    }
}
