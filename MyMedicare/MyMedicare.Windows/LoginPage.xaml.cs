using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bundle_Library;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyMedicare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private UserDetails details;

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!await LoginUser())
            {
                MessageDialog failDialog = new MessageDialog("Username or password is incorrect");
                await failDialog.ShowAsync();
                return;
            }
            MessageDialog successDialog = new MessageDialog("User " + txtUsername.Text + " successfully logged in");
            await successDialog.ShowAsync();
            Bundle b = new Bundle("LoginPage");
            b.putString("USERNAME",txtUsername.Text);
            Frame.Navigate(typeof(MainMenuPage), b);
        }

        private async Task<bool> LoginUser()
        {
            string username;
            char[] password;

            if (!await ReadUserDetails() || details == null)
                return false;
            foreach (User u in details.Users)
            {
                username = txtUsername.Text;
                password = txtPassword.Password.ToCharArray();
                if(!u.Username.Equals(username))
                    continue;
                for(int i = 0; i < password.Length; i++)
                {
                    if (password.Length != u.Password.Length)
                        return false;
                    if (password[i] != u.Password[i])
                        return false;
                }
            }
            return true;
        }

        private async Task<bool> WriteUserDetails(UserDetails details)
        {
            StorageFile file;
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserDetails));
            serializer.WriteObject(ms, details);
            byte[] buffer = ms.ToArray();
            IStorageItem item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("users.medi");
            if (item == null)
            {
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync("users.medi",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("users.medi");
                await file.DeleteAsync();
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync("users.medi",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            await ms.FlushAsync();
            ms.Dispose();
            return true;
        }

        private async Task<bool> ReadUserDetails()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                Stream stream = await folder.OpenStreamForReadAsync("users.medi");
                using (StreamReader reader = new StreamReader(stream))
                {
                    DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.RootName = "root";
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserDetails), settings);
                    details = (UserDetails)serializer.ReadObject(stream);
                }
                return true;
            }
            catch (FileNotFoundException ex)
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("users.medi");
                details = UserDetails.GetInstance();
                return true;
            }
            return false;
        }
    }
}
