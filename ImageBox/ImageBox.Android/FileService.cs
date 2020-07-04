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
            if (!string.IsNullOrEmpty(folderName))
            {
                string _path = Environment.ExternalStorageDirectory.AbsolutePath;

                string _directorySearch = Path.Combine(_path, Environment.DirectoryPictures, folderName);
                if (!Directory.Exists(_directorySearch))
                {
                    Directory.CreateDirectory(_directorySearch);
                }
            }
        }

        public void DeleteFile(string imageName)
        {
            if (File.Exists(imageName))
            {
                File.Delete(imageName);
            }
        }

        public List<DestinationFolder> GetFolders()
        {
            List<DestinationFolder> _folders = new List<DestinationFolder>();
            string _path = Environment.ExternalStorageDirectory.AbsolutePath;

            string _directorySearch = Path.Combine(_path, Environment.DirectoryPictures);

            string[] _directories = Directory.GetDirectories(_directorySearch);

            foreach (string _directory in _directories)
            {
                if (!",temp,trash,".Contains(Path.GetFileNameWithoutExtension(_directory).ToLower()))
                {
                    if (_directory != _path)
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

        public ImageList GetSortedImages(string folderName)
        {
            ImageList _imageList = new ImageList();

            string _path = Environment.ExternalStorageDirectory.AbsolutePath;

            string _directorySearch = Path.Combine(_path, Environment.DirectoryPictures, folderName);
            if (Directory.Exists(_directorySearch))
            {
                var allowedExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
                _imageList.Photos.AddRange(Directory.EnumerateFiles(_directorySearch, "*.*", SearchOption.AllDirectories)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith)));
            }
            
            return _imageList;
        }

        public ImageList GetTrashImages()
        {
            ImageList _imageList = new ImageList();

            string _path = Android.App.Application.Context.FilesDir.AbsolutePath;

            string _directorySearch = Path.Combine(_path, "trash");
            if (!Directory.Exists(_directorySearch))
            {
                Directory.CreateDirectory(_directorySearch);
            }

            var allowedExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
            _imageList.Photos.AddRange(Directory.EnumerateFiles(_directorySearch, "*.*", SearchOption.AllDirectories)
                .Where(file => allowedExtensions.Any(file.ToLower().EndsWith)));            
            
            return _imageList;
        }

        public ImageList GetUnsortedImages()
        {
            ImageList _imageList = new ImageList();

            string _path = Environment.ExternalStorageDirectory.AbsolutePath;

            var _folders = new[] { Environment.DirectoryDcim, Environment.DirectoryDownloads };
            foreach (string folder in _folders)
            {
                string _directorySearch = Path.Combine(_path, folder);
                if (Directory.Exists(_directorySearch))
                {
                    //https://github.com/xamarin/xamarin-android/issues/3426
                    var allowedExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
                    _imageList.Photos.AddRange(Directory.EnumerateFiles(_directorySearch, "*.*", SearchOption.AllDirectories)
                        .Where(file => allowedExtensions.Any(file.ToLower().EndsWith)));
                }
            }
            return _imageList;
        }

        public void MoveFile(string folderName, string imageName)
        {
            string _path = Environment.ExternalStorageDirectory.AbsolutePath;
            string filename = Path.GetFileName(imageName);

            string newdirectory = Path.Combine(_path, folderName);

            if (!Directory.Exists(newdirectory))
            {
                Directory.CreateDirectory(newdirectory);
            }

            filename = Path.Combine(newdirectory, filename);
            File.Move(imageName, filename);
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