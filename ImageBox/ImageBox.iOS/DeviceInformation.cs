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

            storageInfo.TotalSpace = values.Size;
            storageInfo.FreeSpace = values.FreeSize;
            storageInfo.AvailableSpace = values.FreeSize;

            return storageInfo;
        }
    }
}