namespace ImageBox
{
    using System.Collections.Generic;
    using System.IO;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public static class FileManager
    {
        public static void CreateFolder(string folderName)
        {
            DependencyService.Get<IFileService>().CreateFolder(folderName);
        }
        public static void MoveFile(string folderName, string imageName)
        {
            DependencyService.Get<IFileService>().MoveFile(folderName, imageName);
        }
        public async static void MoveFileToTrash(string imageName)
        {
            await App.Database.AddItem(new ImageInfo(imageName));
            DependencyService.Get<IFileService>().MoveFileToTrash(imageName);
        }
        public async static void DeleteFile(string imageName)
        {
            await App.Database.DeleteItem(new ImageInfo(imageName));
            DependencyService.Get<IFileService>().DeleteFile(imageName);
        }
        public static ImageList GetTrashImages()
        {
            return DependencyService.Get<IFileService>().GetTrashImages();
        }
        public static ImageList GetUnsortedImages()
        {
            return DependencyService.Get<IFileService>().GetUnsortedImages();
        }
        public static ImageList GetSortedImages(string folderName)
        {
            return DependencyService.Get<IFileService>().GetSortedImages(folderName);
        }
        public static List<DestinationFolder> GetFolders()
        {
            return DependencyService.Get<IFileService>().GetFolders();
        }
        public async static void RestoreFile(string imageName)
        {
            ImageInfo imageInfo = new ImageInfo(imageName);
            imageInfo = await App.Database.Get(imageInfo.Name);

            string destinationFolder = Path.Combine(FileSystem.AppDataDirectory, "Images");
            if (imageInfo != null)
            {
                destinationFolder = Path.GetDirectoryName(imageInfo.ImagePath);
            }
            DependencyService.Get<IFileService>().RestoreFile(destinationFolder, imageName);
        }

        public static string GetCompressedImage(string imageName, float width, float height)
        {
            string _directoryName = Path.Combine(FileSystem.CacheDirectory, "Cache");
            string _filename = Path.Combine(_directoryName, Path.GetFileName(imageName));
            if (!File.Exists(_filename))
            {
                byte[] byteData = DependencyService.Get<IMediaService>().ResizeImage(imageName, width, height);
                if(byteData != null)
                {
                    if (!Directory.Exists(_directoryName))
                    {
                        Directory.CreateDirectory(_directoryName);
                    }
                    File.WriteAllBytes(_filename, byteData);
                }
            }

            return _filename;
        }

        public static void CheckPermission()
        {
            if (DeviceInfo.Platform.ToString() == Device.Android)
            {
                DependencyService.Get<ICheckFilePermission>().CheckPermission();
            }
        }
    }
}
