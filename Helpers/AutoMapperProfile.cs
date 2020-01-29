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
            CreateMap<UserModel, UserForReturnDTO>()
                .ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
                });
            CreateMap<UserModel, UserForRecordsDTO>()
                .ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
                });
            CreateMap<UserForRegisterDTO, UserForLoginDTO>();
            
            CreateMap<ExerciseForTrainingForCreateDTO, ExerciseForTrainingModel>();
            CreateMap<ExerciseForTrainingForEditDTO, ExerciseForTrainingModel >();
            CreateMap<ExerciseForTrainingModel, ExerciseForTrainingForReturnDTO>().ReverseMap();

            CreateMap<TrainingPlanForCreateDTO, TrainingPlanModel>();
            CreateMap<TrainingPlanModel, TrainingPlanForReturnDTO>();

            CreateMap<TrainingForCreateDTO, TrainingModel>();
            CreateMap<TrainingForEditDTO, TrainingModel>();
            CreateMap<TrainingModel, TrainingForReturnDTO>();

            CreateMap<TrainingPlanModel, TrainingPlanForReturnDTO>();
            CreateMap<TrainingPlanForEditDTO, TrainingPlanModel>();
            
            CreateMap<TrainingModel, TrainingForReturnDTO>();

            CreateMap<MessageForCreateDTO, MessageModel>().ReverseMap();
            CreateMap<MessageModel, MessageForReturnDTO>();

            CreateMap<CommentForReturnDTO, CommentModel>();
            CreateMap<CommentForEditDTO, CommentModel>();
        }
    }
}