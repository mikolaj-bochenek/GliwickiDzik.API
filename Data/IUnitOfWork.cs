using System;
using System.Threading.Tasks;

namespace GliwickiDzik.API.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ILikeRepository Likes { get; }
        ITrainingRepository Trainings { get; }
        IExerciseRepository Exercises { get; }
        IPlanRepository Plans { get; }
        ICommentRepository Comments { get; }
        IMessageRepository Messages { get; }
        Task<bool> SaveAllAsync();
    }
}