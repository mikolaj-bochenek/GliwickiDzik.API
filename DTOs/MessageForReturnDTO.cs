using System;

namespace GliwickiDzik.API.DTOs
{
    public class MessageForReturnDTO
    {
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DataRead { get; set; }
        public DateTime DateSent { get; set; }
        public string MessageContainer { get; set; }
    }
}