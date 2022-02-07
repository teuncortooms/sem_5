using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class StudentsRepository : Repository<Student>
    {
        public StudentsRepository(PersistenceContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Student>()
                //.Include(s => s.Groups)
                .Include(s => s.CurrentGroup)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Student> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Student>()
                .Include(s => s.Groups)
                .Include(s => s.CurrentGroup)
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public override IQueryable<Student> Query()
        {
            return _context.Students
                .Include(s => s.Groups)
                .Include(s => s.CurrentGroup)
                .AsSplitQuery();
        }
    }
}
