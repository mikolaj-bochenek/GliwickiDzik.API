using System.Linq;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;

namespace GliwickiDzik.API.Data
{
    public class CommentRepository : GenericRepository<CommentModel>, ICommentRepository
    {
        public CommentRepository(DataContext _dataContext) : base(_dataContext) {}
        
        public async Task<PagedList<CommentModel>> GetAllCommentsAsync(int trainingPlanId, CommentParams commentParams)
        {
            var comments = _dataContext.CommentModel
                            .Where(c => c.TrainingPlanId == trainingPlanId)
                            .OrderByDescending(c => c.VoteCounter);  

            if(!string.IsNullOrEmpty(commentParams.OrderBy))
            {
                switch(commentParams.OrderBy)
                {
                    case "Newest":
                        comments = comments.OrderByDescending(c => c.DateOfCreated);
                        break;

                    case "Oldest":
                        comments = comments.OrderBy(c => c.DateOfCreated);
                        break;
                        
                    default:
                        comments = comments.OrderByDescending(c => c.VoteCounter);
                        break;
                }
            }
            return await PagedList<CommentModel>.CreateListAsync(comments, commentParams.PageSize, commentParams.PageNumber);                               
        }
    }
}