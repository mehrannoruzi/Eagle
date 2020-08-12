using Elk.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $ext_safeprojectname$.Domain
{
    public class AAA
    {
        [Key]
        public int AAAId { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Desc { get; set; }

        public int FId { get; set; }

        public FileType FileType { get; set; }

        [ForeignKey(nameof(FId))]
        public Action Action { get; set; }

        [NotMapped]
        public Attachment Attachment { get; set; }

    }
}
