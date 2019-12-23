using AutoMapper;
using GliwickiDzik.API.DTOs;
using GliwickiDzik.API.Models;
using GliwickiDzik.DTOs;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Method<what map, map to what>();
            CreateMap<UserForRegisterDTO, UserModel>();
            CreateMap<UserForEditDTO, UserModel>();
            CreateMap<UserModel, UserForUseDTO>();

            CreateMap<ExerciseForTrainingForCreateDTO, ExerciseForTrainingModel>();
            CreateMap<ExerciseForTrainingModel, ExerciseForTrainingForEditDTO>();

            CreateMap<TrainingPlanForCreateDTO, TrainingPlanModel>();

            CreateMap<TrainingForCreateDTO, TrainingModel>();

            CreateMap<TrainingPlanModel, TrainingPlanForReturnDTO>();
            //CreateMap<TrainingPlansForReturnDTO, TrainingPlanModel>();
            CreateMap<TrainingModel, TrainingForReturnDTO>();

        }
    }
}