using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageBox.Pages
{
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


        void LoadBitmapCollection()
        {
            try
            {
                if (!string.IsNullOrEmpty(FolderName))
                {
                    foldertitle.Text = FolderName;

                    flexLayout.Children.Clear();
                    ImageList imageList = CacheDataImages.GetImages(FolderName);

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
                Margin = 5,
                AutomationId = filepath

            };

            flexLayout.Children.Add(image);
        }
    }
}