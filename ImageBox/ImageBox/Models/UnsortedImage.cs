namespace ImageBox
{
    using System.IO;
    public class UnsortedImage
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public bool Selected { get; set; }

        public UnsortedImage() { }

        public UnsortedImage(string imagesource)
        {
            Name = Path.GetFileName(imagesource);
            ImagePath = imagesource;
        }
    }
}
