using Android.OS;
using ImageBox;
using ImageBox.Droid;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

[assembly: Dependency(typeof(FileService))]
namespace ImageBox.Droid
{
    public class FileService : IFileService
    {
        public void CreateFolder(string folderName)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFile(string imageName)
        {
            throw new System.NotImplementedException();
        }

        public List<DestinationFolder> GetFolders()
        {
            throw new System.NotImplementedException();
        }

        public ImageList GetImages(string folderName)
        {
            ImageList _imageList = new ImageList();
            string _path = string.Empty;

            _path = Environment.ExternalStorageDirectory.AbsolutePath;

            var _folders = new[] { Environment.DirectoryDcim, Environment.DirectoryDownloads, Environment.DirectoryPictures };
            foreach (string folder in _folders)
            {
                string _directorySearch = Path.Combine(_path, folder);
                if (Directory.Exists(_directorySearch))
                {
                    //https://github.com/xamarin/xamarin-android/issues/3426
                    var allowedExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
                    _imageList.Photos = Directory.EnumerateFiles(_directorySearch)
                        .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                        .ToList<string>();
                }
            }
            return _imageList;
        }

        public void MoveFile(string folderName, string imageName)
        {
            throw new System.NotImplementedException();
        }

        void IFileService.Save(string name, Stream data, string location)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "Orders", location);
            Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, name);

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (data)
                {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
        }
    }
}