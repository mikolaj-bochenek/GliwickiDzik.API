using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface IContentRepository : IGenericRepository<MessageModel>, IGenericRepository<CommentModel>
    {
         Task<MessageModel> GetMessageAsync(int messageId);
         Task<IEnumerable<MessageModel>> GetAllMessagesAsync();
         Task<IEnumerable<MessageModel>> GetMessageThreadAsync(int userId, int recipientId);
         Task<CommentModel> GetCommentAsync(int commentId);
         Task<IEnumerable<CommentModel>> GetAllCommentsAsync();
         Task<bool> SaveContentAsync();
    }
}