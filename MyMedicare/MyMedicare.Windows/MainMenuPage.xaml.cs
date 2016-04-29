using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MainMenuPage : Page
    {
        private string currentUser = "";
        public MainMenuPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null || !(e.Parameter is Bundle))
                throw new InvalidBundleException("Found Invalid Bundle");
            Bundle b = (Bundle) e.Parameter;
            if(!b.Identifier.Equals("LoginPage"))
                throw new InvalidBundleException("Found Invalid Bundle");
            currentUser = b.getString("USERNAME");
            if(string.IsNullOrEmpty(currentUser))
                throw new ArgumentException("Invalid User logged in");
            Frame.BackStack.Clear();
        }

        private void btnAddNewRecord_Click(object sender, RoutedEventArgs e)
        {
            Bundle b = new Bundle("MainMenuPage");
            b.putString("USERNAME",currentUser);
            Frame.Navigate(typeof(NewRecordPage), b);
        }
    }
}
