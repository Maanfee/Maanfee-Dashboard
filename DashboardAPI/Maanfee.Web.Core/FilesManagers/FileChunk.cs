namespace Maanfee.Web.Core
{
    public class FileChunk
    {
        public string FileName { get; set; } = string.Empty;
        public long Offset { get; set; }
        public byte[] Data { get; set; }

        public bool FirstChunk = false;
    }
}
