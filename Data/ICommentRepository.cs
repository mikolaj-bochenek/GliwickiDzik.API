using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface ICommentRepository : IGenericRepository<CommentModel>
    {
         Task<PagedList<CommentModel>> GetAllCommentsAsync(int trainingPlanId, CommentParams commentParams);
    }
}