namespace ImageBox
{
    public interface IMediaService
    {
        byte[] ResizeImage(string filename, float width, float height);
    }
}
