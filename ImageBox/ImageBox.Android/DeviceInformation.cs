using Android.OS;
using ImageBox;
using ImageBox.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceInformation))]
namespace ImageBox.Droid
{
    public class DeviceInformation : IDeviceInformation
    {
        StorageInfo IDeviceInformation.GetStorage()
        {
            StorageInfo storageInfo = new StorageInfo();

            StatFs stat = new StatFs(Environment.RootDirectory.AbsolutePath);

            SetStats(storageInfo.LocalStorage, stat);

            stat = new StatFs(Environment.ExternalStorageDirectory.AbsolutePath);

            SetStats(storageInfo.SDStorage, stat);

            return storageInfo;
        }

        private void SetStats(StorageBase storage, StatFs stat)
        {
            long totalSpaceBytes = 0;
            long freeSpaceBytes = 0;
            long availableSpaceBytes = 0;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr2)
            {
                totalSpaceBytes = stat.BlockCountLong * stat.BlockSizeLong;
                availableSpaceBytes = stat.AvailableBlocksLong * stat.BlockSizeLong;
                freeSpaceBytes = stat.FreeBlocksLong * stat.BlockSizeLong;
            }
            storage.TotalSpace = totalSpaceBytes;
            storage.AvailableSpace = availableSpaceBytes;
            storage.FreeSpace = freeSpaceBytes;
        }
    }
}