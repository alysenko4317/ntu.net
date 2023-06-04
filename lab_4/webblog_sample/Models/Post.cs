using System;

namespace webblog.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsDraft { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
