namespace ImageBox
{    
    using System.Collections.Generic;
    using System.IO;
    public interface IFileService
    {
        void Save(string name, Stream data, string location = "temp");
        void CreateFolder(string folderName);
        void MoveFile(string folderName, string imageName);
        void DeleteFile(string imageName);
        ImageList GetUnsortedImages();
        ImageList GetSortedImages(string folderName);
        ImageList GetTrashImages();
        List<DestinationFolder> GetFolders();
    }
}
//https://docs.microsoft.com/en-us/xamarin/android/platform/files/