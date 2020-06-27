namespace ImageBox
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Xamarin.Essentials;

    public static class CacheDataImages
    {

        public static void SaveImage(string imageName)
        {
            var cacheDir = FileSystem.CacheDirectory;
            string newdirectory = Path.Combine(cacheDir, "temp");

            if (!Directory.Exists(newdirectory))
            {
                Directory.CreateDirectory(newdirectory);
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

        public static List<string> GetImages(string folderName)
        {
            return new List<string>();
        }

        public static List<DestinationFolder> GetFolders()
        {
            List<DestinationFolder> _folders = new List<DestinationFolder>();

            var cacheDir = FileSystem.CacheDirectory;

            string[] _directories = Directory.GetDirectories(cacheDir);

            foreach (string _directory in _directories)
            {
                if (!Path.GetFileNameWithoutExtension(_directory).ToLower().Equals("temp"))
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
