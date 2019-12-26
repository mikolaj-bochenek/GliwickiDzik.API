using System;
using System.ComponentModel.DataAnnotations;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Models
{
    public class MessageModel
    {
        [Key]
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public UserModel Sender { get; set; }
        public int RecipientId { get; set; }
        public UserModel Recipient { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DataRead { get; set; }
        public DateTime DateSent { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}