using ImageBox.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheckFilePermission))]
namespace ImageBox.Droid
{
    public class CheckFilePermission : ICheckFilePermission
    {
        MainActivity mainActivity;

        public bool CheckPermission()
        {
            if (App.ParentWindow != null)
            {
                mainActivity = (MainActivity)App.ParentWindow;
                if (mainActivity.CheckAppPermissions())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}