using System;

namespace Core.Domain.Entities
{
    public class Course : EntityBase
    {
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
