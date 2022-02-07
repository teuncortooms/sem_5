using System;

namespace Core.Domain.Entities
{
    public class Grade : EntityBase
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public int Score { get; set; }
    }
}
