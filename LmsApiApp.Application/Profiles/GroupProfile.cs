using AutoMapper;
using LmsApiApp.Application.Dtos.GroupDtos;
using LmsApiApp.Core.Entities;

namespace LmsApiApp.Application.Mappers
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            // GroupDto -> Group mapping (Create işlemi için)
            CreateMap<GroupDto, Group>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id otomatik artacak, ignore ediyoruz
                .ForMember(dest => dest.Courses, opt => opt.Ignore()); // Course, CourseId'den gelecek, manuel atama yapılmayacak.

            // Group -> GroupDto mapping (Get işlemleri için)
            CreateMap<Group, GroupDto>();
        }
    }
}
