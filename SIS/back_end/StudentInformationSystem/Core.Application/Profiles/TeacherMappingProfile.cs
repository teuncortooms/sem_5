using Core.Domain.Entities;
using AutoMapper;
using Core.Application.Teachers.Models;

namespace Core.Application.Profiles
{
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile()
        {
            CreateMap<Teacher, TeacherModel>();
        }
    }
}
