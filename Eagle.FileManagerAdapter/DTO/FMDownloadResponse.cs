﻿using Elk.Core;

namespace Eagle.FileManagerAdapter
{
    public class FMDownloadResponse
    {
        public string FileAddress { get; set; }
        public FileType FileType { get; set; }
        public string FileName { get; set; }
    }
}
