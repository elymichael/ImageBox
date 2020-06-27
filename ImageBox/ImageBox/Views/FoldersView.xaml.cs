namespace ImageBox.Views
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoldersView : ContentView
    {
        public FoldersView()
        {
            InitializeComponent();

            this.loadDirectory();
        }

        List<DestinationFolder> DestinationImageFolders { 
            get 
            { 
                return App._destinationImageFolders; 
            } 
            set { 
                App._destinationImageFolders = value; 
            } 
        }

        public delegate void OnMoveFileDelegate(string folderName);

        public OnMoveFileDelegate OnMoveFileClicked { get; set; }

        private void loadDirectory()
        {
            folderLayout.Children.Clear();

            DestinationImageFolders = CacheDataImages.GetFolders();
            foreach (DestinationFolder df in DestinationImageFolders)
            {
                StackLayout stackLayout = new StackLayout()
                {
                    Padding = new Thickness(5),
                    Margin = new Thickness(0, 5, 0, 0),
                    AutomationId = df.Name
                };

                Label labelText = new Label()
                {
                    Text = df.Name,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                Label labelIcon = new Label()
                {
                    Text = ((char)0xf063).ToString(),
                    TextColor = Color.White,
                    FontFamily = (OnPlatform<string>)Application.Current.Resources["FontAwesomeSolid"],
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.NumberOfTapsRequired = 1;
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    string folderName = ((StackLayout)s).AutomationId;
                    if (OnMoveFileClicked != null)
                        OnMoveFileClicked(folderName);
                };
                stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

                stackLayout.Children.Add(labelIcon);
                stackLayout.Children.Add(labelText);
                folderLayout.Children.Add(stackLayout);
            }
            CreateNewButtom();
        }

        private void CreateNewButtom()
        {
            StackLayout stackLayout = new StackLayout()
            {
                Padding = new Thickness(5),
                Margin = new Thickness(0, 5, 0, 0)
            };

            Label labelText = new Label()
            {
                Text = "New Folder",
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            Label labelIcon = new Label()
            {
                Text = ((char)0xf0fe).ToString(),
                TextColor = Color.Gray,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["FontAwesomeSolid"],
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.NumberOfTapsRequired = 1;
            tapGestureRecognizer.Tapped += OnTapGestureRecognizerNewButtonTapped;            
            stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

            stackLayout.Children.Add(labelIcon);
            stackLayout.Children.Add(labelText);
            folderLayout.Children.Add(stackLayout);
        }

        async void OnTapGestureRecognizerNewButtonTapped(object sender, EventArgs args)
        {
            string result = await App.Current.MainPage.DisplayPromptAsync("New Folder", "Add your folder name");
            if (result != null)
            {
                CacheDataImages.CreateFolder(result);
            }
        }
    }
}