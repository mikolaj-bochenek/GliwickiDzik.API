using System;

namespace GliwickiDzik.API.DTOs
{
    public class MessageForCreateDTO
    {
        public MessageForCreateDTO()
        {
            DateOfSent = DateTime.Now;
        }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime DateOfSent { get; set; }
        public string Content { get; set; }

    }
}