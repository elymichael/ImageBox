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
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = this;
        }


        public ICommand ClosePage => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            await Navigation.PopModalAsync();
        }
    }
}