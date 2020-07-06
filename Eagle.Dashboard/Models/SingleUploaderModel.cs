using System;
using Eagle.Domain;
using Microsoft.Extensions.Configuration;

namespace Eagle.Dashboard
{
    public class SingleUploaderModel
    {
        public SingleUploaderModel(IConfiguration configure, Attachment attch)
        {
            if (attch != null)
            {
                AttachmentId = attch.AttachmentId;
                FileId = attch.FileId;
                Type = attch.Type;
                Url = $"{configure["CustomSettings:FileManager:Url"]}/File/DownloadFile?fileId={attch.FileId}&fileType={attch.FileType}";
            }
        }

        public bool HasAttch => !string.IsNullOrWhiteSpace(Url);

        public string AttchName { get; set; }

        public string UploaderName { get; set; }

        public int AttachmentId { get; set; }

        public Guid FileId { get; set; }

        public string Url { get; set; }

        public string FileName { get; set; }

        public AttachmentType Type { get; set; }

        public string Accept { get; set; } = "image/*";
    }
}
