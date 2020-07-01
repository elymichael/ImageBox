namespace ImageBox
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ImageList
    {
        [DataMember(Name = "photos")]
        public List<string> Photos = null;
        public ImageList()
        {
            Photos = new List<string>();
        }
    }
}
