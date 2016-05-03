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
    public sealed partial class ViewRecordsPage : Page
    {
        private string currentUser;
        private UserDetails details;
        private RecordList records;

        public ViewRecordsPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            base.OnNavigatedTo(e);
            if (e.Parameter == null || !(e.Parameter is Bundle))
                throw new InvalidBundleException("Found Invalid Bundle");
            Bundle b = (Bundle)e.Parameter;
            if (!b.Identifier.Equals("MainMenuPage"))
                throw new InvalidBundleException("Found Invalid Bundle Origin");
            currentUser = b.getString("USERNAME");
            if (string.IsNullOrEmpty(currentUser))
                throw new ArgumentException("Invalid User logged in");
            if(!await CreateViewModel())
                throw new InvalidDataException("Could not load records");
        }

        private async Task<bool> CreateViewModel()
        {
            if (!await ReadUserDetails())
            {
                Debug.WriteLine("Could not load user details");
                return false;
            }
            if (!await ReadRecordData())
            {
                Debug.WriteLine("Could not load records");
                return false;
            }
            RecordList visibleRecordList = records;
            if (currentUser.Equals("admin"))
                visibleRecordList = records;
            else
                visibleRecordList.Records = records.Records.Where(x => x.Owner.Username.Equals(currentUser)).ToList();
            RecordViewModel model = new RecordViewModel(records);
            lstRecords.DataContext = model;
            lstRecords.ItemsSource = model.PopulateData();
            return true;
        }

        [Obsolete("Use CreateViewModel Instead",true)]
        private async Task<bool> PopulateRecordList()
        {
            if (!await ReadUserDetails())
            {
                Debug.WriteLine("Could not load user details");
                return false;
            }
            lstRecords.ItemsSource = null;
            if (lstRecords.Items != null) lstRecords.Items.Clear();
            if (!await ReadRecordData())
            {
                Debug.WriteLine("Could not load records");
                return false;
            }
            if (currentUser.Equals("admin"))
                lstRecords.ItemsSource = records.Records;
            else
                lstRecords.ItemsSource = records.Records.Where(x => x.Owner.Username.Equals(currentUser));
            return true;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
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
                    details = (UserDetails)serializer.ReadObject(stream);
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

        private async Task<bool> ReadRecordData()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                Stream stream = await folder.OpenStreamForReadAsync("records.dat");
                if (stream.Length > 0)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                        settings.RootName = "root";
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RecordList),
                            settings);
                        records = (RecordList) serializer.ReadObject(stream);
                    }
                }
                return true;
            }
            catch (FileNotFoundException ex)
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("records.dat");
                records = RecordList.GetInstance();
                return true;
            }
            return false;
        }

        private async Task<User> GetCurrentUser()
        {
            if (!await ReadUserDetails())
            {
                Debug.WriteLine("Error occurred while reading users");
                return null;
            }
            foreach (User u in details.Users)
            {
                if (u.Username.Equals(currentUser))
                    return u;
            }
            return null;
        }
    }
}
