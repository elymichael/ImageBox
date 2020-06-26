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

        ObservableCollection<DestinationFolder> _destinationFolder = new ObservableCollection<DestinationFolder>();
        ObservableCollection<DestinationFolder> ImagesDestinationFolder { get { return _destinationFolder; } }

        public FoldersView()
        {
            InitializeComponent();

            this.loadDirectory();
        }

        private void loadDirectory()
        {
            List<DestinationFolder> folders = ImageFolderScan.GetFolders("");
            foreach (DestinationFolder df in folders)
            {
                _destinationFolder.Add(df);
            }
        }
    }
}