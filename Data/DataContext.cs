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
        public DbSet<LikeModel> LikeModel { get; set; }
        public DbSet<NewTraining> NewTraining { get; set; }

         protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LikeModel>().HasKey(k => new { k.UserIdLikesPlanId, k.PlanIdIsLikedByUserId });

            builder.Entity<LikeModel>().HasOne(u => u.PlanIsLiked)
                                    .WithMany(u => u.UserLikes)
                                    .HasForeignKey(u => u.PlanIdIsLikedByUserId)
                                    .OnDelete(DeleteBehavior.Restrict);
                        
            builder.Entity<LikeModel>().HasOne(u => u.UserLikes)
                                    .WithMany(u => u.PlanIsLiked)
                                    .HasForeignKey(u => u.UserIdLikesPlanId)
                                    .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<MessageModel>().HasOne(u => u.Sender)
                                    .WithMany(m => m.MessagesSent)
                                    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MessageModel>().HasOne(u => u.Recipient)
                                    .WithMany(m => m.MessagesReceived)
                                    .OnDelete(DeleteBehavior.Restrict);
        }

    }
}