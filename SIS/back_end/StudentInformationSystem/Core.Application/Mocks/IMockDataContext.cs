using Core.Domain.Entities;
using System.Collections.Generic;

namespace Core.Application.Mocks
{
    public interface IMockDataContext
    {
        List<T> GetEntities<T>() where T : EntityBase;
    }
}
