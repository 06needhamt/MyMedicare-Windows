using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFormData())
            {
                Debug.WriteLine("An Error occurred while registering");
            }
        }

        private bool CheckFormData()
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
                dialog.ShowAsync();
                return false;
            }
            return true;
        }
    }
}
