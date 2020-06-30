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
        ImageList GetImages(string folderName);
        List<DestinationFolder> GetFolders();
    }
}
//https://docs.microsoft.com/en-us/xamarin/android/platform/files/