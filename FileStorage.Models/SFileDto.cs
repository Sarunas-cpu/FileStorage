using System;

namespace FileStorage.Models
{
    public class SFileDto
    {
        public int? ParentId { get; set; }
        public int? RootId { get; set; }
        public string Name { get; set; }
        public long? CreatedBy { get; set; }
        public int IsDeleted { get; set; }
        public string FileContents { get; set; }
    }
}
