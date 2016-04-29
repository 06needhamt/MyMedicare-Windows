using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
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
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyMedicare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewRecordPage : Page
    {
        private string currentUser;
        private EnumTemperatureUnit temperatureUnit;
        private double temperature;
        private double bpHigh;
        private double bpLow;
        private double heartRate;
        private UserDetails details;
        private RecordList records;

        public NewRecordPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null || !(e.Parameter is Bundle))
                throw new InvalidBundleException("Found Invalid Bundle");
            Bundle b = (Bundle)e.Parameter;
            if (!b.Identifier.Equals("MainMenuPage"))
                throw new InvalidBundleException("Found Invalid Bundle");
            currentUser = b.getString("USERNAME");
            if (string.IsNullOrEmpty(currentUser))
                throw new ArgumentException("Invalid User logged in");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Record r;
            //#if DEBUG // TODO Remove
            //    StorageFile file;
            //    IStorageItem item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("users.dat");
            //    if (item != null)
            //    {
            //        file = await ApplicationData.Current.LocalFolder.GetFileAsync("users.dat");
            //        await file.DeleteAsync();
            //    }
            //#endif

            if (!await CheckFormInput())
            {
                MessageDialog dialog = new MessageDialog("Please Ensure All Fields Are Filled");
                await dialog.ShowAsync();
                return;
            }
            r = await CreateRecord();
            if (r == null)
            {
                Debug.WriteLine("An Error occurred while creating the record");
                return;    
            }
            if (!await ReadRecordData() && records == null)
            {
                Debug.WriteLine("No records found or an error occurred");
                return;
            }
            records.AddRecord(r);
            if (!await WriteRecordData(records))
            {
                Debug.WriteLine("An Error occurred while writing records");
            }
            MessageDialog riskDialog = new MessageDialog("Your Risk Level is: " + r.RiskLevel.ToString());
            await riskDialog.ShowAsync();
            Frame.GoBack();

        }

        private async Task<bool> CheckFormInput()
        {
            if (rdbCelcius.IsChecked != null &&
                (rdbFahrenheit.IsChecked != null && (!rdbCelcius.IsChecked.Value && !rdbFahrenheit.IsChecked.Value)))
            {
                MessageDialog dialog = new MessageDialog("Please Select a Temperature Unit");
                await dialog.ShowAsync();
                return false;
            }
            if (txtTemperature.Text.Equals("") || Convert.ToDouble(txtTemperature.Text) < 0)
                return false;
            if (txtBloodPressureHigh.Text.Equals("") || Convert.ToDouble(txtBloodPressureHigh.Text) < 0)
                return false;
            if (txtBloodPressureLow.Text.Equals("") || Convert.ToDouble(txtBloodPressureLow.Text) < 0)
                return false;
            if (txtHeartRate.Text.Equals("") || Convert.ToDouble(txtHeartRate.Text) < 0)
                return false;
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
                using (StreamReader reader = new StreamReader(stream))
                {
                    DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.RootName = "root";
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RecordList), settings);
                    records = (RecordList)serializer.ReadObject(stream);
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

        private async Task<bool> WriteRecordData(RecordList records)
        {
            StorageFile file;
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RecordList));
            serializer.WriteObject(ms, records);
            byte[] buffer = ms.ToArray();
            IStorageItem item = await ApplicationData.Current.LocalFolder.TryGetItemAsync("records.dat");
            if (item == null)
            {
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync("records.dat",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("records.dat");
                await file.DeleteAsync();
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync("records.dat",
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

        private async Task<Record> CreateRecord()
        {
            if (rdbCelcius.IsChecked != null && rdbCelcius.IsChecked.Value)
                temperatureUnit = EnumTemperatureUnit.CELCIUS;
            else if (rdbFahrenheit.IsChecked != null && rdbFahrenheit.IsChecked.Value)
                temperatureUnit = EnumTemperatureUnit.FAHRENHEIT;
            else
                throw new ArgumentException("Invalid Temperature Unit Found");
            temperature = Convert.ToDouble(txtTemperature.Text);
            bpHigh = Convert.ToDouble(txtBloodPressureHigh.Text);
            bpLow = Convert.ToDouble(txtBloodPressureLow.Text);
            heartRate = Convert.ToDouble(txtHeartRate.Text);
            User owner = await GetCurrentUser();
            if (owner == null)
                return null;
            EnumRiskLevel riskLevel = await CalculateRiskLevel();
            Record record = new Record(owner,riskLevel,temperatureUnit,temperature,bpLow,bpHigh,heartRate,DateTime.Now);
            record.RiskLevel = await CalculateRiskLevel();
            return record;
        }
        private async Task<EnumRiskLevel> CalculateRiskLevel()
        {
            if (temperatureUnit == EnumTemperatureUnit.FAHRENHEIT)
            {
                if (temperature <= 98.6 && bpLow < 80 && bpHigh < 120 && heartRate <= 72)
                    return EnumRiskLevel.LOW;
                else if ((temperature >= 98.6 && temperature <= 100.4) &&
                        (bpLow >= 0 && bpLow < 110) &&
                        (bpHigh >= 0 && bpHigh < 180)
                        && heartRate < 160)
                    return EnumRiskLevel.MEDIUM;
                else
                    return EnumRiskLevel.HIGH;
            }
            else
            {
                if (temperature <= 37 && bpLow < 80 && bpHigh < 120 && heartRate <= 72)
                    return EnumRiskLevel.LOW;
                else if ((temperature >= 37 && temperature <= 38) &&
                        (bpLow >= 0 && bpLow < 110) &&
                        (bpHigh >= 0 && bpHigh < 180)
                        && heartRate < 160)
                    return EnumRiskLevel.MEDIUM;
                else
                    return EnumRiskLevel.HIGH;
            }

        }
    }
}
