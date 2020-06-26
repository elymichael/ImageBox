namespace ImageBox
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;

    public static class ImageFolderScan
    {
        // Class for deserializing JSON list of sample bitmaps

        /// <summary>
        /// Return the images related to path indicated in the paramenters. For testing purpose use the image json.
        /// </summary>
        /// <param name="imagepath">"https://raw.githubusercontent.com/xamarin/docs-archive/master/Images/stock/small/stock.json"</param>
        /// <returns>Return an Image List with all images found.</returns>
        public static async Task<ImageList> GetImages(string imagepath)
        {
            ImageList imageList = new ImageList();

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    // Download the list of stock photos
                    Uri uri = new Uri(imagepath);
                    byte[] data = await webClient.DownloadDataTaskAsync(uri);

                    // Convert to a Stream object
                    using (Stream stream = new MemoryStream(data))
                    {
                        // Deserialize the JSON into an ImageList object
                        var jsonSerializer = new DataContractJsonSerializer(typeof(ImageList));
                        imageList = (ImageList)jsonSerializer.ReadObject(stream);

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error was found: " + ex.Message);
                }
            }
            return imageList;
        }

        public static List<DestinationFolder> GetFolders(string imagepath)
        {
            List<DestinationFolder> _destinationFolder = new List<DestinationFolder>();

            _destinationFolder.Add(new DestinationFolder { Name = "Clases", FolderType = FolderType.Images, Quantity = 20, });
            _destinationFolder.Add(new DestinationFolder { Name = "Familia", FolderType = FolderType.Images, Quantity = 5 });
            _destinationFolder.Add(new DestinationFolder { Name = "Música", FolderType = FolderType.Images, Quantity = 2 });
            _destinationFolder.Add(new DestinationFolder { Name = "Meditación", FolderType = FolderType.Images, Quantity = 12 });
            _destinationFolder.Add(new DestinationFolder { Name = "Investigación", FolderType = FolderType.Images, Quantity = 20 });
            _destinationFolder.Add(new DestinationFolder { Name = "Personal", FolderType = FolderType.Images, Quantity = 300 });
            _destinationFolder.Add(new DestinationFolder { Name = "Otros", FolderType = FolderType.Images, Quantity = 55 });
            _destinationFolder.Add(new DestinationFolder { Name = "Hijos", FolderType = FolderType.Images, Quantity = 450 });

            return _destinationFolder;
        }
    }
}
