using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Application.Students.Models;
using Core.Domain.Entities;

namespace Core.Application.Profiles
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile(IRepository<Student> repository)
        {
            CreateMap<Student, StudentViewModel>()
                .ReverseMap()
                .PreserveReferences()
                .ConstructUsing(o => repository.GetAsync(o.Id, default).Result);
        }

        public StudentMappingProfile()
        {
            CreateMap<Student, StudentModel>()
                .ForCtorParam("student", c => c.MapFrom(src => src));
            CreateMap<Student, StudentDetailsModel>()
                .ForCtorParam("student", c => c.MapFrom(src => src));
            CreateMap<Student, MinimalStudentModel>();
        }
    }
}
