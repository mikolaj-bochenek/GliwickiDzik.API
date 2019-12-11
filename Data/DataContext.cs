using GliwickiDzik.API.Models;
using GliwickiDzik.Models;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base (options) {}
        
        public DbSet<UserModel> UserModel { get; set; }
        public DbSet<PhotoModel> PhotoModel { get; set; }
        public DbSet<CommentModel> CommentModel { get; set; }
        public DbSet<ExerciseForTrainingModel> ExerciseForTrainingModel { get; set; }
        public DbSet<ExerciseModel> ExerciseModel { get; set; }
        public DbSet<MessageModel> MessageModel { get; set; }
        public DbSet<TrainingPlanModel> TrainingPlanModel { get; set; }
        public DbSet<TrainingModel> TrainingModel { get; set; }

    }
}