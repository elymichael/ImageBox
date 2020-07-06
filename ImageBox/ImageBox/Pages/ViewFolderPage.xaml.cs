namespace ImageBox.Pages
{
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using FFImageLoading.Forms;
    using System.Threading.Tasks;

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

        public event EventHandler<EventArgs> OperationCompleted;

        public ICommand CloseTrash => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
        }

        private int rowPosition = 0;
        private int colPosition = 0;
        async void LoadBitmapCollection()
        {
            try
            {
                if (!string.IsNullOrEmpty(FolderName))
                {                    
                    await Task.Run(async () =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            foldertitle.Text = FolderName;
                            flexLayout.Children.Clear();
                        });

                        ImageList imageList = FileManager.GetSortedImages(FolderName);

                        rowPosition = 0;
                        colPosition = 0;
                        lblTotalImages.Text = string.Format("Photo(s): {0}", imageList.Photos.Count);
                        foreach (string filepath in imageList.Photos)
                        {
                            await AddImage(filepath);
                        }
                    });
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
                
            }
        }

        private async Task AddImage(string filepath)
        {
            string imagePath = await Task<string>.Factory.StartNew(() => FileManager.GetCompressedImage(filepath, 150, 150));

            Device.BeginInvokeOnMainThread(async () =>
            {
                CachedImage image = new CachedImage
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 120,
                    WidthRequest = 120,
                    Aspect = Aspect.Fill,
                    AutomationId = filepath,
                    IsOpaque = true,
                    BackgroundColor = Color.Gray
                };
                image.DownsampleToViewSize = true;
                image.CacheDuration = new TimeSpan(5, 0, 0, 0);
                image.Source = await Task<ImageSource>.Factory.StartNew(() => ImageSource.FromFile(imagePath));

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.NumberOfTapsRequired = 2;
                tapGestureRecognizer.Tapped += OnTapGestureRecognizerFolderTapped;
                image.GestureRecognizers.Add(tapGestureRecognizer);

                Grid.SetColumn(image, colPosition);
                Grid.SetRow(image, rowPosition);

                flexLayout.Children.Add(image);

                colPosition++;
                if (colPosition == 3)
                {
                    colPosition = 0;
                    rowPosition++;
                }
            });
        }

        private async void OnTapGestureRecognizerFolderTapped(object sender, EventArgs args)
        {
            try
            {
                CachedImage image = (CachedImage)sender;
                image.Opacity = 0.2;

                var options = new[] { "Unsort Photo", "Move to trash", "View Image" };
                string action = await DisplayActionSheet("Pick a choice...", "Cancel", null, options);

                switch (action)
                {
                    case "Move to trash":
                        FileManager.MoveFileToTrash(image.AutomationId);
                        LoadBitmapCollection();
                        break;
                    case "Unsort Photo":
                        FileManager.RestoreFile(image.AutomationId);
                        LoadBitmapCollection();
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "An error has ocurred: " + ex.Message, "OK");
            }
        }

    }
}