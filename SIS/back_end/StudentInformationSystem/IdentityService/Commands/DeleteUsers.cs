using AutoMapper;
using Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Commands
{
    public static class DeleteUsers
    {
        public class Command : IRequest
        {
            public ICollection<string> Ids { get; set; }
        }

        private class Handler : IRequestHandler<Command>
        {
            private readonly UserManager<AppUser> userManager;
            private readonly IMapper mapper;

            public Handler(UserManager<AppUser> userManager, IMapper mapper)
            {
                this.userManager = userManager;
                this.mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                foreach(var Id in request.Ids)
                {
                    var user = await userManager.FindByIdAsync(Id).ConfigureAwait(false);
                    await userManager.DeleteAsync(user).ConfigureAwait(false);
                }

                return Unit.Value;
            }
        }
    }
}
