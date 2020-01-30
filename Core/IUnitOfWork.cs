using System;
using System.Threading.Tasks;
using GliwickiDzik.API.Core.Interfaces;

namespace GliwickiDzik.API.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IExeRepository Exes { get; }
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