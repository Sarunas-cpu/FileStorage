using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FileStorage.Database.Entities
{
    public class SFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? RootId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public int IsDeleted { get; set; }
    }
}
