namespace ImageBox
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public static class FileManager
    {
        public async static void CreateFolder(string folderName)
        {
            await App.Database.AddFolderItem(folderName);
            DependencyService.Get<IFileService>().CreateFolder(folderName);
        }
        public static void MoveFile(string folderName, string imageName)
        {
            DependencyService.Get<IFileService>().MoveFile(folderName, imageName);
        }
        public async static void MoveFileToTrash(string imageName)
        {
            await App.Database.AddImageItem(new ImageInfo(imageName));
            DependencyService.Get<IFileService>().MoveFileToTrash(imageName);
        }
        public async static void DeleteFile(string imageName)
        {
            await App.Database.DeleteImageItem(new ImageInfo(imageName));
            DependencyService.Get<IFileService>().DeleteFile(imageName);

            // Delete Cache Image file.
            string _directoryName = Path.Combine(FileSystem.CacheDirectory, "Cache");
            string _filename = Path.Combine(_directoryName, Path.GetFileName(imageName));
            if (File.Exists(_filename))
            {
                File.Delete(_filename);
            }
        }
        public static ImageList GetTrashImages()
        {
            return DependencyService.Get<IFileService>().GetTrashImages();
        }
        public async static Task<ImageList> GetUnsortedImages()
        {
            List<FolderInfo> folderList = await App.Database.GetFolders();
            return DependencyService.Get<IFileService>().GetUnsortedImages(folderList);
        }
        public static ImageList GetSortedImages(string folderName)
        {
            return DependencyService.Get<IFileService>().GetSortedImages(folderName);
        }
        public async static Task<List<DestinationFolder>> GetFolders()
        {
            List<FolderInfo> folderList = await App.Database.GetFolders();
            return DependencyService.Get<IFileService>().GetFolders(folderList);
            //return folders.Where(x => folderList.Exists(y => y.Name.ToLower() == x.Name.ToLower())).ToList();            
        }
        public async static void RestoreFile(string imageName)
        {
            ImageInfo imageInfo = new ImageInfo(imageName);
            imageInfo = await App.Database.GetImage(imageInfo.Name);

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
                if (byteData != null)
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
