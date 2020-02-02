using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Helpers.Params;
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
        public DbSet<ExerciseModel> ExerciseModel { get; set; }
        public DbSet<MessageModel> MessageModel { get; set; }
        public DbSet<PlanModel> TrainingPlanModel { get; set; }
        public DbSet<TrainingModel> TrainingModel { get; set; }
        public DbSet<LikeModel> LikeModel { get; set; }

         protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PlanModel>().HasMany(p => p.Trainings)
                    .WithOne(t => t.Plan)
                    .HasForeignKey(k => k.PlanId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserModel>().HasKey(k => k.UserId);

            builder.Entity<PlanModel>().HasKey(k => k.PlanId);

            builder.Entity<TrainingModel>().HasKey(k => k.TrainingId);
            
            builder.Entity<ExerciseModel>().HasKey(k => k.ExerciseId);

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
            
            //builder.Entity<ExerciseForTraining>().HasNoKey();
        }

    }
}