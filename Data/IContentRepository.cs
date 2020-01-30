using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface IContentRepository : IGenericRepository<MessageModel>, IGenericRepository<CommentModel>
    {
         
         Task<CommentModel> GetCommentAsync(int commentId);
         Task<PagedList<CommentModel>> GetAllCommentsAsync(int trainingPlanId, CommentParams commentParams);
         Task<bool> SaveContentAsync();
    }
}