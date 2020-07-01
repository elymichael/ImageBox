namespace ImageBox.Pages
{    
    using System.Windows.Input; 
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            
            loadConfiguration();
        }

        private void loadConfiguration()
        {
            BindingContext = new SettingsModelView();
        }
    }
}