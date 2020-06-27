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

            List<DestinationFolder> folders = CacheDataImages.GetFolders();
            foreach (DestinationFolder df in folders)
            {
                df.Images = CacheDataImages.GetImages(df.Name);
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
            string result = await App.Current.MainPage.DisplayPromptAsync("New Folder", "Add your folder name");
            if (result != null)
            {
                CacheDataImages.CreateFolder(result);
                loadDirectory();
            }            
        }


        public ICommand ViewFolder => new Command(OnViewButtonClicked);

        private async void OnViewButtonClicked()
        {
            await DisplayAlert("Mensaje", "tab", "Ok");
        }

        async void OnTapGestureRecognizerFolderTapped(object sender, EventArgs args)
        {
            string folderName = ((StackLayout)sender).AutomationId;

            var viewFolderPage = new ViewFolderPage(folderName);
            
            await Navigation.PushModalAsync(viewFolderPage, true);
        }
    }
}