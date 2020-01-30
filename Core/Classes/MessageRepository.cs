using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Data
{
    public class MessageRepository : GenericRepository<MessageModel>, IMessageRepository
    {
        
        public MessageRepository(DataContext dataContext) : base(dataContext) {}
        
        public async Task<PagedList<MessageModel>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var messages = _dataContext.MessageModel.Include(u => u.Sender)
                                            .Include(u => u.Recipient).AsQueryable();
            
            switch (messageParams.MessageContainer)
            {
                case "Inbox" :
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;
                case "Outbox" :
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;
                default :
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false && u.RecipientDeleted == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.DateOfSent);

            return await PagedList<MessageModel>.CreateListAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }
        
        public async Task<IEnumerable<UserModel>> GetConvMessagesAsync(int userId)
        {
            var messages = await _dataContext.MessageModel.Where(m => m.SenderId == userId || m.RecipientId == userId)
                .OrderByDescending(m => m.DateOfSent).ToListAsync();

            var listOfUsers = new List<UserModel>();

            foreach (var item in messages)
            {
                if (item.SenderId == userId)
                {
                    if (!listOfUsers.Contains(item.Recipient))
                        listOfUsers.Add(item.Recipient);
                }
                
                if (item.RecipientId == userId)
                {
                    if (!listOfUsers.Contains(item.Sender))
                        listOfUsers.Add(item.Sender);
                }
            }
            return listOfUsers;
        }

        public async Task<IEnumerable<MessageModel>> GetMessageThreadAsync(int userId, int recipientId)
        {
            return await _dataContext.MessageModel
                //.Include(s => s.SenderId == userId)
                //.Include(r => r.RecipientId == recipientId)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false
                         || m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false)
                .OrderByDescending(m => m.DateOfSent).ToListAsync();
        }
    }
}