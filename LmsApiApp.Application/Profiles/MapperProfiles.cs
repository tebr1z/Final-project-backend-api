

using AutoMapper;
using LmsApiApp.Application.Dtos.AssignmentDtos;
using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Application.Dtos.GroupDtos;
using LmsApiApp.Application.Dtos.TestDtos;
using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Core.Entities;

namespace LmsApiApp.Application.Profiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Course, CourseDto>()
      .ForMember(dest => dest.UserIds, opt => opt.MapFrom(src => src.CourseTeachers.Select(ct => ct.UserId).ToList()))
    // Burada UserId kullanılıyor
      .ReverseMap();


            // Diğer dönüşümler
     
            CreateMap<Assignment, AssignmentDto>()
                       .ReverseMap();
            CreateMap<UpdateAssignmentDto, Assignment>()
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  // CreatedAt'i güncellemeye dahil etmiyoruz
           .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.MediaUrl, opt => opt.Condition(src => src.MediaUrl != null));
            CreateMap<AssignmentSubmission, AssignmentSubmissionDto>().ReverseMap();

            CreateMap<AssignmentSubmissionDto, AssignmentSubmission>()
          .ForMember(dest => dest.MediaUrl, opt => opt.Ignore())  // MediaUrl dosya yüklemeden sonra ayarlanacak
          .ForMember(dest => dest.SubmittedAt, opt => opt.Ignore());  // Gönderim tarihi otomatik ayarlanacak

            CreateMap<AssignmentSubmission, AssignmentSubmissionResponseDto>()
        .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade));

            CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<LmsApiApp.Application.Dtos.TestDtos.TestDto, Test>()
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Test, TestResponseDto>();

            // QuestionDto'dan Question'a eşleme
            CreateMap<QuestionDto, Question>()
                .ForMember(dest => dest.Answers, opt =>
                    opt.MapFrom(src => src.Answers.Select(a => new Answer
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList())); // Cevapları burada eşleştiriyoruz

            // Question'dan QuestionDto'ya eşleme
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.Answers, opt =>
                    opt.MapFrom(src => src.Answers)); // Cevapları buradan eşleştiriyoruz

            // AnswerDto'dan Answer'a eşleme
            CreateMap<AnswerDto, Answer>();

            // Answer'dan AnswerDto'ya eşleme
            CreateMap<Answer, AnswerDto>();


            CreateMap<Test, TestResponseDto>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions)); // Soruları dahil et

            CreateMap<Question, QuestionDto>(); // Soruları da haritalayın
            CreateMap<Answer, AnswerDto>();


            // TestResult'dan TestResultDto'ya haritalama
            CreateMap<TestResult, TestResultDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt));

            // TestResultDto'dan TestResult'a haritalama
            CreateMap<TestResultDto, TestResult>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<User, UserDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
