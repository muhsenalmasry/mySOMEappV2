using System;
using System.Collections.Generic;

namespace mySOMEappV2.Models
{
    public partial class Message
    {
        public Guid Id { get; set; }
        public DateTime? Time { get; set; }
        public string Content { get; set; } = null!;
        public Guid Sender { get; set; }
        public Guid Reciever { get; set; }

        public virtual User RecieverNavigation { get; set; } = null!;
        public virtual User SenderNavigation { get; set; } = null!;
    }
}
