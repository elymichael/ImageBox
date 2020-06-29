using Xamarin.Forms;
using ImageBox;
using ImageBox.iOS;
using Foundation;
using System;

[assembly: Dependency(typeof(DeviceInformation))]
namespace ImageBox.iOS
{
    public class DeviceInformation : IDeviceInformation
    {
        public StorageInfo GetStorage()
        {
            StorageInfo storageInfo = new StorageInfo();
            NSFileSystemAttributes values = NSFileManager.DefaultManager.GetFileSystemAttributes(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

            storageInfo.localStorage.TotalSpace = values.Size;
            storageInfo.localStorage.FreeSpace = values.FreeSize;
            storageInfo.localStorage.AvailableSpace = values.FreeSize;

            return storageInfo;
        }
    }
}