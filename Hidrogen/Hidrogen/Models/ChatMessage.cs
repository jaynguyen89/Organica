using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ChatMessage
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public int GroupId { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
        public string IsHiddenFor { get; set; }
        public bool IsVisible { get; set; }
        public string Attachment { get; set; }
        public string SeenByIds { get; set; }

        public virtual ChatGroup Group { get; set; }
        public virtual ChatParticipant Participant { get; set; }
    }
}
