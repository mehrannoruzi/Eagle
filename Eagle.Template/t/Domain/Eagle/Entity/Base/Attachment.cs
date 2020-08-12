using System;
using Elk.Core;
using $ext_safeprojectname$.Domain.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $ext_safeprojectname$.Domain
{
    [Table(nameof(Attachment), Schema = "Content")]
    public class Attachment : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }

        //[Display(Name = nameof(Strings.RecordId), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        public int RecordId { get; set; }

        //[Display(Name = nameof(Strings.FileId), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        public Guid FileId { get; set; }

        //[Display(Name = nameof(Strings.TableName), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        public TableName TableName { get; set; }

        //[Display(Name = nameof(Strings.FileType), ResourceType = typeof(Strings))]
        public FileType FileType { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        //[Display(Name = nameof(Strings.AttachmentType), ResourceType = typeof(Strings))]
        public AttachmentType Type { get; set; }

        [Column(TypeName = "nvarchar(120)")]
        //[Display(Name = nameof(Strings.FileName), ResourceType = typeof(Strings))]
        public string FileName { get; set; }

        [Column(TypeName = "nvarchar(120)")]
       // [Display(Name = nameof(Strings.OriginalName), ResourceType = typeof(Strings))]
        public string FileOriginalName { get; set; }

        //[Display(Name = nameof(Strings.FileSize), ResourceType = typeof(Strings))]
        public int Size { get; set; }
    }

}