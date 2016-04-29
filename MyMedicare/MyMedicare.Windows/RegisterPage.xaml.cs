using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyMedicare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private UserDetails details;

        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!await RegisterNewUser())
            {
                Debug.WriteLine("An Error occurred while registering");
            }
            MessageDialog dialog = new MessageDialog("User " + txtUsername.Text + " Successfully Registered");
            await dialog.ShowAsync();
            Frame.GoBack();
        }

        private async Task<bool> RegisterNewUser()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string firstname = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string age = txtAge.Text;
            string address1 = txtAddress1.Text;
            string address2 = txtAddress2.Text;
            string gpName = txtgpName.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string ReEnterPassword = txtReEnterPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(age) || string.IsNullOrEmpty(address1) ||
                string.IsNullOrEmpty(gpName) || string.IsNullOrEmpty(phoneNumber) ||
                string.IsNullOrEmpty(ReEnterPassword)) // if a required field is not filled
            {
                MessageDialog dialog = new MessageDialog("All Fields Except Address 2 are required");
                await dialog.ShowAsync();
                return false;
            }
            if (Convert.ToInt32(age) < 0)
            {
                MessageDialog dialog = new MessageDialog("Age can not be negative");
                await dialog.ShowAsync();
                return false;
            }
            if (phoneNumber.Length != 11)
            {
                MessageDialog dialog = new MessageDialog("Phone number must be 11 digits");
                await dialog.ShowAsync();
                return false;
            }
            if (!password.Equals(ReEnterPassword))
            {
                MessageDialog dialog = new MessageDialog("Passwords do not match");
                await dialog.ShowAsync();
                return false;
            }
            if (!await ReadUserDetails() || details == null)
            {
                Debug.WriteLine("No user details found or an error occurred");
                details = UserDetails.GetInstance();
            }
            if (await UserNameExists(username))
            {
                Debug.WriteLine("User " + username + " already exists");
                return false;
            }
            User newUser = new User(username,password.ToCharArray(),firstname,lastName,
                Convert.ToInt32(age),address1,address2,phoneNumber,gpName);
            details.AddUser(newUser);
            if (!await WriteUserDetails(details))
            {
                Debug.WriteLine("Error writing user details");
                return false;
            }
            return true;
        }

        private async Task<bool> UserNameExists(string username)
        {
            foreach (User u in details.Users)
            {
                if (u.Username.Equals(username))
                {
                    MessageDialog dialog = new MessageDialog("Username " + username + " already exists");
                    await dialog.ShowAsync();
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> WriteUserDetails(UserDetails details)
        {
            StorageFile file;
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserDetails));
            serializer.WriteObject(ms,details);
            byte[] buffer = ms.ToArray();
            IStorageItem item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("users.dat");
            if (item == null)
            {
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync("users.dat",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("users.dat");
                await file.DeleteAsync();
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync("users.dat",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            await ms.FlushAsync();
            ms.Dispose();
            #if DEBUG
                string text = await FileIO.ReadTextAsync(file);
                string[] split = text.Split(new char[] { ',' });
                foreach (string str in split)
                {
                    Debug.WriteLine(str);
                }
            #endif
            return true;
        }

        private async Task<bool> ReadUserDetails()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                Stream stream = await folder.OpenStreamForReadAsync("users.dat");
                using (StreamReader reader = new StreamReader(stream))
                {
                    DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.RootName = "root";
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserDetails), settings);
                    details = (UserDetails) serializer.ReadObject(stream);
                }
                return true;
            }
            catch (FileNotFoundException ex)
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("users.dat");
                details = UserDetails.GetInstance();
                return true;
            }
            return false;
        }
    }
}
