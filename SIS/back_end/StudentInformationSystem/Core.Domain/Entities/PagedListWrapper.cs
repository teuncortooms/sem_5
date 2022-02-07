using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Entities
{
    public class PagedListWrapper<T>
    {
        public ICollection<T> Results { get; set; }
        public int ResultCount => Results?.Count ?? 0;
        public int TotalCount { get; set; }

    }
}
