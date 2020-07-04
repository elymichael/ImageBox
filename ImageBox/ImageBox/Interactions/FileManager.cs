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
        public static void DeleteFile(string imageName)
        {
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
    }
}
