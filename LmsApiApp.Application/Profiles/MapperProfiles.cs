

using AutoMapper;
using LmsApiApp.Application.Dtos.AssignmentDtos;
using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Application.Dtos.GroupDtos;
using LmsApiApp.Core.Entities;

namespace LmsApiApp.Application.Profiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            CreateMap<Course, CourseDto>().ReverseMap();

            CreateMap<Group, GroupDto>()
                .ForMember(dest => dest.CourseIds, opt => opt.MapFrom(src => src.Courses.Select(c => c.Id)));

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

        }
    }
}
