using AutoMapper;
using Core.Application.Groups.Models;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.Entities;
using System;
using System.Linq.Dynamic.Core;

namespace Core.Application.Profiles
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile(IRepository<Group> repository)
        {
            CreateMap<Group, GroupViewModel>()
            .ReverseMap()
            .PreserveReferences()
            .ConstructUsing(o => repository.GetAsync(o.Id, default).Result);
        }

        public GroupMappingProfile()
        {
            CreateMap<Group, GroupModel>()
                .ForCtorParam("group", c => c.MapFrom(src => src));
            CreateMap<Group, GroupDetailsModel>()
                .ForCtorParam("group", c => c.MapFrom(src => src));
            CreateMap<Group, MinimalGroupModel>()
                .ForCtorParam("group", c => c.MapFrom(src => src));
        }
    }
}
