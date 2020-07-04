
using ImageBox;
using ImageBox.iOS;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace ImageBox.iOS
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

        public ImageList GetUnsortedImages()
        {
            throw new System.NotImplementedException();
        }

        public ImageList GetSortedImages(string folderName)
        {
            throw new System.NotImplementedException();
        }

        public void MoveFile(string folderName, string imageName)
        {
            throw new System.NotImplementedException();
        }

        public void Save(string name, Stream data, string location = "temp")
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
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

        public ImageList GetTrashImages()
        {
            throw new NotImplementedException();
        }
    }
}