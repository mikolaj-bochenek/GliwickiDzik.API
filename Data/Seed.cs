using System.Runtime.Serialization;
using System.IO;
using GliwickiDzik.Data;
using Newtonsoft.Json;
using GliwickiDzik.Models;
using System.Collections.Generic;
using System.Text;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public class Seed
    {
        private readonly DataContext _dataContext;

        public Seed(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void SeedData()
        {
            var userData = File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<UserModel>>(userData);

            //var trainingPlanData = File.ReadAllText("Data/TrainingPlanSeedData.json");
            //var trainingPlans = JsonConvert.DeserializeObject<List<TrainingPlanModel>>(trainingPlanData);

            //var trainingData = File.ReadAllText("Data/TrainingSeedData.json");
            //var trainings = JsonConvert.DeserializeObject<List<TrainingModel>>(trainingData);

            //var exerciseData = File.ReadAllText("Data/ExerciseSeedData.json");
            //var exercises = JsonConvert.DeserializeObject<List<ExerciseForTrainingModel>>(exerciseData);

            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;

                GetPasswordHashAndSalt("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _dataContext.UserModel.Add(user);
            }
            _dataContext.SaveChanges();
        }
        private void GetPasswordHashAndSalt(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        }
    }
