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
                        $"AppName:     {AppInfo.Name}\n" +
                        $"AppBuild:     {AppInfo.BuildString}\n" +
                        $"Device:     {DeviceInfo.Model}\n" +
                        $"Manufacturer:     {DeviceInfo.Manufacturer}\n" +
                        $"DeviceName:     {DeviceInfo.Name}\n" +
                        $"Version:        {DeviceInfo.VersionString}\n" +
                        $"Platform:      {DeviceInfo.Platform}\n" +
                        $"Idiom:    {DeviceInfo.Idiom}\n" +
                        $"DeviceType:     {DeviceInfo.DeviceType}\n"+
                        $"Package:     {AppInfo.PackageName}\n";

            lblApplicationInfo.Text = (geocodeAddress);
            
            StorageInfo storage = DependencyService.Get<IDeviceInformation>().GetStorage();

            lblPrimaryStorage.Text = bytesToHuman(storage.FreeSpace) + " of " + bytesToHuman(storage.TotalSpace);
        }

        public ICommand ClosePage => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            await Navigation.PopModalAsync();
        }

        private static string bytesToHuman(double size)
        {
            long Kb = 1 * 1024;
            long Mb = Kb * 1024;
            long Gb = Mb * 1024;
            long Tb = Gb * 1024;
            long Pb = Tb * 1024;
            long Eb = Pb * 1024;

            if (size < Kb) return (size).ToString("#.##") + " byte";
            if (size >= Kb && size < Mb) return ((double)size / Kb).ToString("#.##") + " Kb";
            if (size >= Mb && size < Gb) return ((double)size / Mb).ToString("#.##") + " Mb";
            if (size >= Gb && size < Tb) return ((double)size / Gb).ToString("#.##") + " Gb";
            if (size >= Tb && size < Pb) return ((double)size / Tb).ToString("#.##") + " Tb";
            if (size >= Pb && size < Eb) return ((double)size / Pb).ToString("#.##") + " Pb";
            if (size >= Eb) return ((double)size / Eb).ToString("#.##") + " Eb";

            return "???";
        }
    }
}