namespace ImageBox
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public static class FileManager
    {
        public static async Task SaveUrlImage(string imageName)
        {
            await SaveUrlImage("temp", imageName);            
        }

        public static async Task SaveUrlImage(string folderName, string imageName)
        {
            var cacheDir = FileSystem.CacheDirectory;

            string filename = Path.GetFileName(imageName);

            string newdirectory = Path.Combine(cacheDir, folderName);

            if (!Directory.Exists(newdirectory))
            {
                Directory.CreateDirectory(newdirectory);
            }

            filename = Path.Combine(newdirectory, filename);
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(imageName))
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    File.WriteAllBytes(filename, imageBytes);
                    // Store in the in phone public directory.
                    Stream stream = new MemoryStream(imageBytes);
                    DependencyService.Get<IFileService>().Save(Path.GetFileName(filename), stream, folderName);
                }
            }
        }

        public static void CreateFolder(string folderName)
        {
            if (!string.IsNullOrEmpty(folderName))
            {
                var cacheDir = FileSystem.CacheDirectory;
                string newdirectory = Path.Combine(cacheDir, folderName);

                if (!Directory.Exists(newdirectory))
                {
                    Directory.CreateDirectory(newdirectory);
                }
            }
        }

        public static void MoveFile(string folderName, string imageName)
        {
            string filename = Path.GetFileName(imageName);
            string newdirectory = Path.Combine(FileSystem.CacheDirectory, folderName);
            if (!Directory.Exists(newdirectory))
            {
                Directory.CreateDirectory(newdirectory);
            }

            filename = Path.Combine(newdirectory, filename);
            File.Move(imageName, filename);
        }

        public static void DeleteFile(string imageName)
        {
            if (File.Exists(imageName))
            {
                File.Delete(imageName);
            }
        }

        public static ImageList GetImages(string folderName)
        {
            string newdirectory = Path.Combine(FileSystem.CacheDirectory, folderName);
            ImageList imageList = new ImageList();
            imageList.Photos = Directory.GetFiles(newdirectory).ToList<string>();

            return imageList;

        }

        public static List<DestinationFolder> GetFolders()
        {
            List<DestinationFolder> _folders = new List<DestinationFolder>();

            var cacheDir = FileSystem.CacheDirectory;

            string[] _directories = Directory.GetDirectories(cacheDir);

            foreach (string _directory in _directories)
            {
                if (!",temp,trash,".Contains(Path.GetFileNameWithoutExtension(_directory).ToLower()))
                {
                    if (_directory != cacheDir)
                    {
                        _folders.Add(new DestinationFolder()
                        {
                            Name = Path.GetFileNameWithoutExtension(_directory),
                            FolderType = FolderType.Images
                        });
                    }
                }
            }
            return _folders;
        }
    }
}
