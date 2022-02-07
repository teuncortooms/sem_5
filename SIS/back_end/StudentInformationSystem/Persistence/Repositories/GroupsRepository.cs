using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GroupsRepository : Repository<Group>
    {
        public GroupsRepository(PersistenceContext context) : base(context)
        {
        }

        //public override async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Group>().Include(g => g.Students).ToListAsync(cancellationToken);
        //}

        public override async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Group>()
                .Include(s => s.Students)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Group> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Group>().Include(g => g.Students).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}
