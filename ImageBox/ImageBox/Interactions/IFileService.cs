namespace ImageBox
{    
    using System.Collections.Generic;
    using System.IO;
    public interface IFileService
    {        
        void CreateFolder(string folderName);
        void MoveFile(string folderName, string imageName);
        void MoveFileToTrash(string imageName);
        void DeleteFile(string imageName);
        ImageList GetUnsortedImages(List<FolderInfo> fi);
        ImageList GetSortedImages(string folderName);
        ImageList GetTrashImages();
        List<DestinationFolder> GetFolders(List<FolderInfo> fi);
        void RestoreFile(string destinationFolder, string imageName);
    }
}
//https://docs.microsoft.com/en-us/xamarin/android/platform/files/