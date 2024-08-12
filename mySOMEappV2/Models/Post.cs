using System;
using System.Collections.Generic;

namespace mySOMEappV2.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public byte[]? Media { get; set; }
        public DateTime? Time { get; set; }
        public int? Likes { get; set; }
        public Guid? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
