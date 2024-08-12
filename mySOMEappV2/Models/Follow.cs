using System;
using System.Collections.Generic;

namespace mySOMEappV2.Models
{
    public partial class Follow
    {
        public Guid Id { get; set; }
        public Guid? Follower { get; set; }
        public Guid? Following { get; set; }

        public virtual User? FollowerNavigation { get; set; }
        public virtual User? FollowingNavigation { get; set; }
    }
}
