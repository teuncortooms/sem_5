using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using FluentValidation;
using IdentityService.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Validation
{
    public class ClaimsValidator : IClaimsValidator
    {
        private readonly ClaimsPrincipal user;

        public ClaimsValidator(ClaimsPrincipal user)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user));
        }
        
        public bool HasGodPermission()
        {
            return user.HasClaim(c => c.Type == "SIS-Claims" && c.Value == UserClaims.All);
        }

        public Guid GetStudentId()
        {
            string value = user.Claims.FirstOrDefault(c => c.Type == UserClaims.StudentId)?.Value;
            if (!Guid.TryParse(value, out Guid studentId)) throw new KeyNotFoundException("Invalid student id");
            return studentId;
        }
    }
}
