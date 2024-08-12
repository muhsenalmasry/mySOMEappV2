using System;
using System.Collections.Generic;

namespace mySOMEappV2.Models
{
    public partial class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? Time { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PostId { get; set; }

        public virtual Post? Post { get; set; }
        public virtual User? User { get; set; }
    }
}
