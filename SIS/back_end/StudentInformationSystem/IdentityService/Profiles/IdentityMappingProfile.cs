using AutoMapper;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Profiles
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<AppUser, AppUserModel>()
                .ForMember(m => m.EMail, o => o.MapFrom(target => target.Email))
                .ForMember(m => m.LoginName, o => o.MapFrom(target => target.UserName))
                .ForMember(m => m.Claims, o => o.Ignore());

            CreateMap<Claim, AppClaimModel>()
                .ForMember(m => m.Name, o=>o.MapFrom(target=>target.Value))
                .ForMember(m => m.Id, o=>o.MapFrom(target=>
                new Guid(Array.FindIndex(UserClaims.Permissions,0,(c)=>c.Equals(target.Value)),0,0,1,1,1,1,1,1,1,1)))
                .ReverseMap()
                .ConstructUsing(c => UserClaims.Claim(c.Name));

            CreateMap<IdentityRole, AppRoleModel>()
                .ForMember(m => m.Name, o => o.MapFrom(target => target.Name))
                .ForMember(m => m.RoleClaims, o => o.Ignore());
        }
    }
}
