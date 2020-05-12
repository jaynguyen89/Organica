using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ChatParticipant
    {
        public ChatParticipant()
        {
            ChatMessage = new HashSet<ChatMessage>();
        }

        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public int GroupId { get; set; }
        public int? AddedById { get; set; }
        public string TimeStamps { get; set; }

        public virtual ChatGroup Group { get; set; }
        public virtual Hidrogenian Participant { get; set; }
        public virtual ICollection<ChatMessage> ChatMessage { get; set; }
    }
}
