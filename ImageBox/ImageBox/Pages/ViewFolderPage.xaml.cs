namespace ImageBox.Pages
{
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewFolderPage : ContentPage
    {
        public ViewFolderPage()
        {            
            InitializeComponent();

            BindingContext = this;

            LoadBitmapCollection();
        }

        public ViewFolderPage(string folderName)
        {
            InitializeComponent();

            BindingContext = this;

            FolderName = folderName;

            LoadBitmapCollection();
        }

        public string FolderName { get; set; }

        public ICommand CloseTrash => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            await Navigation.PopModalAsync();
        }

        private int rowPosition = 0;
        private int colPosition = 0;
        void LoadBitmapCollection()
        {
            try
            {
                if (!string.IsNullOrEmpty(FolderName))
                {
                    foldertitle.Text = FolderName;

                    flexLayout.Children.Clear();
                    ImageList imageList = CacheDataImages.GetImages(FolderName);

                    rowPosition = 0;
                    colPosition = 0;
                    lblTotalImages.Text = string.Format("Photo(s): {0}", imageList.Photos.Count);
                    foreach (string filepath in imageList.Photos)
                    {
                        AddImage(filepath);
                    }
                }
            }
            catch (Exception ex)
            {
                flexLayout.Children.Add(new Label
                {
                    Text = string.Format("Cannot access list of bitmap files: {0}", ex.Message)
                });
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        private void AddImage(string filepath)
        {            
            Image image = new Image
            {
                Source = ImageSource.FromFile(filepath),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 120,
                WidthRequest = 120,
                Aspect = Aspect.Fill,
                Margin = 5,
                AutomationId = filepath
            };
            
            Grid.SetColumn(image, colPosition);
            Grid.SetRow(image, rowPosition);

            flexLayout.Children.Add(image);

            colPosition++;
            if(colPosition == 3)
            {
                colPosition = 0;
                rowPosition++;
            }
        }
    }
}