using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GliwickiDzik.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseModel",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseModel", x => x.ExerciseId);
                });

            migrationBuilder.CreateTable(
                name: "NewTraining",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewTraining", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    DateOfCreated = table.Column<DateTime>(nullable: false),
                    LastActive = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Growth = table.Column<float>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    BicepsSize = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "MessageModel",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SenderId = table.Column<int>(nullable: false),
                    RecipientId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    DateOfRead = table.Column<DateTime>(nullable: true),
                    DateOfSent = table.Column<DateTime>(nullable: false),
                    SenderDeleted = table.Column<bool>(nullable: false),
                    RecipientDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageModel", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_MessageModel_UserModel_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageModel_UserModel_SenderId",
                        column: x => x.SenderId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhotoModel",
                columns: table => new
                {
                    PhotoId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateOfAdded = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoModel", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_PhotoModel_UserModel_UserId",
                        column: x => x.UserId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlanModel",
                columns: table => new
                {
                    TrainingPlanId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateOfCreated = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Owner = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    IsMain = table.Column<bool>(nullable: false),
                    LikeCounter = table.Column<int>(nullable: false),
                    CommentCounter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlanModel", x => x.TrainingPlanId);
                    table.ForeignKey(
                        name: "FK_TrainingPlanModel_UserModel_UserId",
                        column: x => x.UserId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentModel",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommenterId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DateOfCreated = table.Column<DateTime>(nullable: false),
                    TrainingPlanId = table.Column<int>(nullable: false),
                    VoteCounter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentModel", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_CommentModel_UserModel_CommenterId",
                        column: x => x.CommenterId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentModel_TrainingPlanModel_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlanModel",
                        principalColumn: "TrainingPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikeModel",
                columns: table => new
                {
                    UserIdLikesPlanId = table.Column<int>(nullable: false),
                    PlanIdIsLikedByUserId = table.Column<int>(nullable: false),
                    TrainingPlanModelTrainingPlanId = table.Column<int>(nullable: true),
                    UserModelUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeModel", x => new { x.UserIdLikesPlanId, x.PlanIdIsLikedByUserId });
                    table.ForeignKey(
                        name: "FK_LikeModel_TrainingPlanModel_PlanIdIsLikedByUserId",
                        column: x => x.PlanIdIsLikedByUserId,
                        principalTable: "TrainingPlanModel",
                        principalColumn: "TrainingPlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeModel_TrainingPlanModel_TrainingPlanModelTrainingPlanId",
                        column: x => x.TrainingPlanModelTrainingPlanId,
                        principalTable: "TrainingPlanModel",
                        principalColumn: "TrainingPlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeModel_UserModel_UserIdLikesPlanId",
                        column: x => x.UserIdLikesPlanId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeModel_UserModel_UserModelUserId",
                        column: x => x.UserModelUserId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingModel",
                columns: table => new
                {
                    TrainingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Day = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateOfCreated = table.Column<DateTime>(nullable: false),
                    TrainingPlanId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingModel", x => x.TrainingId);
                    table.ForeignKey(
                        name: "FK_TrainingModel_TrainingPlanModel_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlanModel",
                        principalColumn: "TrainingPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseForTrainingModel",
                columns: table => new
                {
                    ExerciseForTrainingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Sets = table.Column<int>(nullable: false),
                    Reps = table.Column<int>(nullable: false),
                    TrainingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseForTrainingModel", x => x.ExerciseForTrainingId);
                    table.ForeignKey(
                        name: "FK_ExerciseForTrainingModel_TrainingModel_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "TrainingModel",
                        principalColumn: "TrainingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewExercise",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExerciseId = table.Column<int>(nullable: false),
                    Sets = table.Column<int>(nullable: false),
                    Reps = table.Column<int>(nullable: false),
                    NewTrainingId = table.Column<int>(nullable: true),
                    TrainingModelTrainingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewExercise_NewTraining_NewTrainingId",
                        column: x => x.NewTrainingId,
                        principalTable: "NewTraining",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewExercise_TrainingModel_TrainingModelTrainingId",
                        column: x => x.TrainingModelTrainingId,
                        principalTable: "TrainingModel",
                        principalColumn: "TrainingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_CommenterId",
                table: "CommentModel",
                column: "CommenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_TrainingPlanId",
                table: "CommentModel",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseForTrainingModel_TrainingId",
                table: "ExerciseForTrainingModel",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeModel_PlanIdIsLikedByUserId",
                table: "LikeModel",
                column: "PlanIdIsLikedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeModel_TrainingPlanModelTrainingPlanId",
                table: "LikeModel",
                column: "TrainingPlanModelTrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeModel_UserModelUserId",
                table: "LikeModel",
                column: "UserModelUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageModel_RecipientId",
                table: "MessageModel",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageModel_SenderId",
                table: "MessageModel",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_NewExercise_NewTrainingId",
                table: "NewExercise",
                column: "NewTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_NewExercise_TrainingModelTrainingId",
                table: "NewExercise",
                column: "TrainingModelTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoModel_UserId",
                table: "PhotoModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingModel_TrainingPlanId",
                table: "TrainingModel",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanModel_UserId",
                table: "TrainingPlanModel",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentModel");

            migrationBuilder.DropTable(
                name: "ExerciseForTrainingModel");

            migrationBuilder.DropTable(
                name: "ExerciseModel");

            migrationBuilder.DropTable(
                name: "LikeModel");

            migrationBuilder.DropTable(
                name: "MessageModel");

            migrationBuilder.DropTable(
                name: "NewExercise");

            migrationBuilder.DropTable(
                name: "PhotoModel");

            migrationBuilder.DropTable(
                name: "NewTraining");

            migrationBuilder.DropTable(
                name: "TrainingModel");

            migrationBuilder.DropTable(
                name: "TrainingPlanModel");

            migrationBuilder.DropTable(
                name: "UserModel");
        }
    }
}
