
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
            throw new NotImplementedException();
        }

        public void DeleteFile(string imageName)
        {
            throw new NotImplementedException();
        }

        public List<DestinationFolder> GetFolders()
        {
            throw new NotImplementedException();
        }

        public ImageList GetSortedImages(string folderName)
        {
            throw new NotImplementedException();
        }

        public ImageList GetTrashImages()
        {
            throw new NotImplementedException();
        }

        public ImageList GetUnsortedImages()
        {
            throw new NotImplementedException();
        }

        public void MoveFile(string folderName, string imageName)
        {
            throw new NotImplementedException();
        }

        public void MoveFileToTrash(string imageName)
        {
            throw new NotImplementedException();
        }

        void IFileService.RestoreFile(string destinationFolder, string imageName)
        {
            throw new NotImplementedException();
        }
    }
}