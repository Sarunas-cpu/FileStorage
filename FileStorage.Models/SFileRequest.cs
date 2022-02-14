using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Models
{
    public class SFileRequest
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? RootId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public int IsDeleted { get; set; }
        public string FileContents { get; set; }
    }
}
