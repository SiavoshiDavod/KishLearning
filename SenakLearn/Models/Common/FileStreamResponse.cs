using System.IO;

namespace SenakLearn.Models.Common
{
    public class FileStreamResponse
    {
        public MemoryStream Stream { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string PathFull { get; set; }
    }
}