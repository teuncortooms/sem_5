using System;

namespace Core.Application.Models
{
    public class GradeViewModel
    {
        public StudentViewModel Student { get; set; }
        public GroupViewModel Group { get; set; }
        public int Score { get; set; }
        public Guid Id { get; set; }
    }
}
