namespace ETClient.API.Response
{
    public class FbImageData
    {
        public int height { get; set; }
        public bool is_silhouette { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class FbImage
    {
        public FbImageData data { get; set; }
    }
}