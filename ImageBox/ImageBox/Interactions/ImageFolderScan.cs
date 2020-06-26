namespace ImageBox
{
    using System;    
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
        public static async Task<ImageList> LoadBitmapCollection(string imagepath)
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
    }
}
