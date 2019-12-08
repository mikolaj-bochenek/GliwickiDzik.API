using GliwickiDzik.API.Models;
using GliwickiDzik.Models;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base (options) {}
        
        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<PhotoModel> PhotoModel { get; set; }
        public DbSet<CommentModel> CommentModel { get; set; }
        public DbSet<ExerciseForTrainingModel> ExerciseForTrainingModel { get; set; }
        public DbSet<ExerciseModel> ExerciseModel { get; set; }
        public DbSet<LikeModel> LikeModel { get; set; }
        public DbSet<MessageModel> MessageModel { get; set; }
        public DbSet<TrainingPlanModel> TrainingPlanModel { get; set; }

    }
}