namespace ImageBox
{
    using System.IO;
    public class ImageInfo
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public bool Selected { get; set; }

        public ImageInfo() { }

        public ImageInfo(string imagesource)
        {
            Name = Path.GetFileName(imagesource);
            ImagePath = imagesource;
        }
    }
}
