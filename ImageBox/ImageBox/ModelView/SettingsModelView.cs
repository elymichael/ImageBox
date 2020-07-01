namespace ImageBox
{
    using PropertyChanged;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;    

    [AddINotifyPropertyChangedInterface]
    public class SettingsModelView
    {
        public string DeviceInformation { get; set; }

        public string PrimaryStorage { get; set; }

        public string SecondaryStorage { get; set; }

        public SettingsModelView()
        {
            var _deviceInformation =
                        $"AppName: {AppInfo.Name}\n" +
                        $"AppBuild: {AppInfo.BuildString}\n" +
                        $"Device: {DeviceInfo.Model}\n" +
                        $"Manufacturer: {DeviceInfo.Manufacturer}\n" +
                        $"DeviceName: {DeviceInfo.Name}\n" +
                        $"Version: {DeviceInfo.VersionString}\n" +
                        $"Platform: {DeviceInfo.Platform}\n" +
                        $"Idiom: {DeviceInfo.Idiom}\n" +
                        $"DeviceType: {DeviceInfo.DeviceType}\n" +
                        $"Package: {AppInfo.PackageName}\n";

            DeviceInformation = (_deviceInformation);

            StorageInfo _storage = DependencyService.Get<IDeviceInformation>().GetStorage();

            PrimaryStorage = _storage.LocalStorage.FreeSpace.bytesToHuman() + " of " + _storage.LocalStorage.TotalSpace.bytesToHuman(); ;
            SecondaryStorage = _storage.SDStorage.FreeSpace.bytesToHuman() + " of " + _storage.SDStorage.TotalSpace.bytesToHuman();
        }

        public ICommand ClosePage => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
