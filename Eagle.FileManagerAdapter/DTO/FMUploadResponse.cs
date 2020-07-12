using Elk.Core;
using System;

namespace Eagle.FileManagerAdapter
{
    public class FMUploadResponse
    {
        public Guid FileID { get; set; }
        public FileType FileType { get; set; }
        public string FileName { get; set; }
        public string FileOriginalName { get; set; }
        public string Tags { get; set; }
        public int Size { get; set; }
    }
}
