using System.Threading.Tasks;
using GliwickiDzik.API.Core.Classes;
using GliwickiDzik.API.Core.Interfaces;
using GliwickiDzik.Data;

namespace GliwickiDzik.API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
            Messages = new MessageRepository(_dataContext);
            Comments = new CommentRepository(_dataContext);
            Plans = new PlanRepository(_dataContext);
            Exercises = new ExerciseRepository(_dataContext);
            Trainings = new TrainingRepository(_dataContext);
            Likes = new LikeRepository(_dataContext);
            Users = new UserRepository(_dataContext);
            Exes = new ExeRepository(_dataContext);
        }
        public IMessageRepository Messages { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public IPlanRepository Plans { get; private set; }
        public IExerciseRepository Exercises { get; private set; }
        public ITrainingRepository Trainings { get; private set; }
        public ILikeRepository Likes { get; private set; }
        public IUserRepository Users { get; private set; }
        public IExeRepository Exes { get; private set; }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
        
        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}