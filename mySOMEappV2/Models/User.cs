using System;
using System.Collections.Generic;

namespace mySOMEappV2.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FollowFollowerNavigations = new HashSet<Follow>();
            FollowFollowingNavigations = new HashSet<Follow>();
            MessageRecieverNavigations = new HashSet<Message>();
            MessageSenderNavigations = new HashSet<Message>();
            Posts = new HashSet<Post>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? Birthday { get; set; }
        public byte[]? ProfilePic { get; set; }
        public byte[]? ProfileCover { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follow> FollowFollowerNavigations { get; set; }
        public virtual ICollection<Follow> FollowFollowingNavigations { get; set; }
        public virtual ICollection<Message> MessageRecieverNavigations { get; set; }
        public virtual ICollection<Message> MessageSenderNavigations { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
