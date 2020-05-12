using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ChatGroup
    {
        public ChatGroup()
        {
            ChatMessage = new HashSet<ChatMessage>();
            ChatParticipant = new HashSet<ChatParticipant>();
        }

        public int Id { get; set; }
        public int? ThingId { get; set; }
        public byte GroupType { get; set; }
        public string GroupTitle { get; set; }
        public string GroupAvatar { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual ICollection<ChatMessage> ChatMessage { get; set; }
        public virtual ICollection<ChatParticipant> ChatParticipant { get; set; }
    }
}
