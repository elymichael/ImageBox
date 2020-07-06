namespace ImageBox
{
    using SQLite;
    using System;    
    using System.IO;
    public class ImageInfo: IComparable<ImageInfo>
    {
        [PrimaryKey]
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public bool Selected { get; set; }

        public ImageInfo() { }

        public ImageInfo(string imagesource)
        {
            Name = Path.GetFileName(imagesource);
            ImagePath = imagesource;
        }

        public int CompareTo(ImageInfo other)
        {
            if (other == null)
                return 1;
            else
                return this.Name.CompareTo(other.Name);
        }
    }
}
