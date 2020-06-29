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
            //StatFs stat = new StatFs(path); //"/storage/sdcard1"
            long totalSpaceBytes = 0;
            long freeSpaceBytes = 0;
            long availableSpaceBytes = 0;


            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr2)
            {
                totalSpaceBytes = stat.BlockCountLong * stat.BlockSizeLong;
                availableSpaceBytes = stat.AvailableBlocksLong * stat.BlockSizeLong;
                freeSpaceBytes = stat.FreeBlocksLong * stat.BlockSizeLong;
            }
            else
            {
                totalSpaceBytes = (long)stat.BlockCount * (long)stat.BlockSize;
                availableSpaceBytes = (long)stat.AvailableBlocks * (long)stat.BlockSize;
                freeSpaceBytes = (long)stat.FreeBlocks * (long)stat.BlockSize;
            }

            storageInfo.TotalSpace = totalSpaceBytes;
            storageInfo.AvailableSpace = availableSpaceBytes;
            storageInfo.FreeSpace = freeSpaceBytes;
            return storageInfo;
        }
    }
}