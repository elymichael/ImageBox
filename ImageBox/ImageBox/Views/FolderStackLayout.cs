namespace ImageBox.Views
{
    using Xamarin.Forms;
    public class FolderStackLayout: StackLayout
    {
        public FolderStackLayout() : base()
        {
            Padding = new Thickness(5);
            Margin = new Thickness(0, 5, 0, 0);

        }

        public void AddLabels(string folderName, Color color, int icon)
        {
            AutomationId = folderName;

            Label labelText = new Label()
            {
                Text = folderName,
                TextColor = color,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            Label labelIcon = new Label()
            {
                Text = ((char)icon).ToString(),
                TextColor = color,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["FontAwesomeSolid"],
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            Children.Add(labelIcon);
            Children.Add(labelText);
        }
    }
}
