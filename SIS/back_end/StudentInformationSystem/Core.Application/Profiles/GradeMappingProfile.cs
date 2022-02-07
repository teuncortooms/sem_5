using AutoMapper;
using Core.Application.Grades.Commands;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.Entities;

namespace Core.Application.Profiles
{
    public class GradeMappingProfile : Profile
    {

        public GradeMappingProfile(IRepository<Grade> repository)
        {
            CreateMap<UpdateGrade.Command, Grade>()
            .PreserveReferences()
            .ConstructUsing(o => repository.GetAsync(o.Id, default).Result);
        }

        public GradeMappingProfile()
        {
            CreateMap<Grade, GradeViewModel>();
            CreateMap<AddGrade.Command, Grade>();
        }
    }
}
