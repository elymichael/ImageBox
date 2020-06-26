namespace ImageBox.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewFolderList : ContentPage
    {
        public ViewFolderList()
        {
            InitializeComponent();

            BindingContext = this;

            loadDirectory();
        }

        ObservableCollection<DestinationFolder> _destinationFolder = new ObservableCollection<DestinationFolder>();
        ObservableCollection<DestinationFolder> ImagesDestinationFolder { get { return _destinationFolder; } }

        private void loadDirectory()
        {
            ImageDestinationDisplay.ItemsSource = _destinationFolder;

            List<DestinationFolder> folders = ImageFolderScan.GetFolders("");
            foreach (DestinationFolder df in folders)
            {
                _destinationFolder.Add(df);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var settingsPage = new SettingsPage();
            await Navigation.PushModalAsync(settingsPage);
        }


        public ICommand ClosePage => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            await Navigation.PopModalAsync();
        }

        public ICommand AddFolder => new Command(OnAddButtonClicked);

        private async void OnAddButtonClicked()
        {
            await Navigation.PopModalAsync();
        }


        public ICommand ViewFolder => new Command(OnViewButtonClicked);

        private async void OnViewButtonClicked()
        {
            await DisplayAlert("Mensaje", "tab", "Ok");
        }
    }
}