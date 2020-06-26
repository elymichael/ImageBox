namespace ImageBox
{
    public enum FolderType
    {
        Videos = 0,
        Images = 1
    }
    public class DestinationFolder
    {
        public string Name { get; set; }

        public FolderType FolderType { get; set; }

        public string Source { get; set; }

        public int Quantity { get; set; }
       
    }
}
