using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IContextAsync<T>
    {
        public Task<int> CountAsync(CancellationToken cancellationToken);
        public Task<T> SingleAsync(CancellationToken cancellationToken);
        public Task<List<T>> ToListAsync(CancellationToken cancellationToken);
    }
}
