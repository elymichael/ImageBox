namespace ImageBox
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using PropertyChanged;
    using Xamarin.Forms;

    [AddINotifyPropertyChangedInterface]
    public class TrashModelView
    {
        public event EventHandler<EventArgs> OperationCompleted;

        public ICommand CloseTrash => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        public string RecoverText { get; set; }

        public string DeleteText { get; set; }

        public bool ActivityIndicatorIsRunning { get; set; }
    }
}
