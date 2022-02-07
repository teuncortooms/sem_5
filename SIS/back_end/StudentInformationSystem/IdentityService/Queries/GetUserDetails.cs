using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Queries;
using IdentityService.Queries.ResponseModels;
using MediatR;

namespace IdentityService.Queries
{
    public class GetUserDetails
    {
        public class Query : IRequest<UserDetailsModel> { }

        public class Handler : IRequestHandler<Query, UserDetailsModel>
        {
            public async Task<UserDetailsModel> Handle(Query request, CancellationToken cancellationToken = default)
            {
                var response = new UserDetailsModel();

                //TODO: NOT CURRENTLY USED!

                //foreach (var c in User.Claims)
                //{
                //    response.Claims.Add(c.Type, c.Value);
                //}

                return await Task.Run(() => response, cancellationToken);
            }
        }
    }
}