using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Data
{
    public interface IMessageRepository : IGenericRepository<MessageModel>
    {
        Task<IEnumerable<UserModel>> GetCorrespondedUsersAsync(int userId);
        Task<PagedList<MessageModel>> GetMessagesForUserAsync(MessageParams messageParams);
        Task<IEnumerable<MessageModel>> GetMessageThreadAsync(int userId, int recipientId);
    }
}