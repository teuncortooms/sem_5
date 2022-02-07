using Core.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ContextAsync<T> : IContextAsync<T>
    {
        private readonly IQueryable<T> query;

        public ContextAsync(IQueryable<T> query)
        {
            this.query = query;
        }
        public async Task<T> SingleAsync(CancellationToken cancellationToken)
        {
            return await query.SingleAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<T>> ToListAsync(CancellationToken cancellationToken)
        {
            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await query.CountAsync(cancellationToken).ConfigureAwait(false);
        }

    }
}
