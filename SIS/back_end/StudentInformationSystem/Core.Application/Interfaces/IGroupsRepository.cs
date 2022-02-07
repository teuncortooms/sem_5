using Core.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IGroupsRepository : IRepository<Group>
    {
        public Task<Group> AddStudentToGroupAsync(Student student, CancellationToken cancellationToken = default);
    }
}
