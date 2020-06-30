namespace ImageBox.Pages
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = this;

            loadConfiguration();
        }

        private void loadConfiguration()
        {
            var geocodeAddress =
                        $"AppName: {AppInfo.Name}\n" +
                        $"AppBuild: {AppInfo.BuildString}\n" +
                        $"Device: {DeviceInfo.Model}\n" +
                        $"Manufacturer: {DeviceInfo.Manufacturer}\n" +
                        $"DeviceName: {DeviceInfo.Name}\n" +
                        $"Version: {DeviceInfo.VersionString}\n" +
                        $"Platform: {DeviceInfo.Platform}\n" +
                        $"Idiom: {DeviceInfo.Idiom}\n" +
                        $"DeviceType: {DeviceInfo.DeviceType}\n"+
                        $"Package: {AppInfo.PackageName}\n";

            lblApplicationInfo.Text = (geocodeAddress);
            
            StorageInfo storage = DependencyService.Get<IDeviceInformation>().GetStorage();

            lblPrimaryStorage.Text = storage.LocalStorage.FreeSpace.bytesToHuman() + " of " + storage.LocalStorage.TotalSpace.bytesToHuman(); ;
            lblSecondaryStorage.Text = storage.SDStorage.FreeSpace.bytesToHuman() + " of " + storage.SDStorage.TotalSpace.bytesToHuman();
            
        }

        public ICommand ClosePage => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            await Navigation.PopModalAsync();
        }
    }
}