namespace ImageBox
{
    using SQLite;
    
    public class FolderInfo
    {
        [PrimaryKey]
        public string Name { get; set; }
    }
}
