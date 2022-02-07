using Core.Application.Groups.Models;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Core.Application.Teachers.Models
{
    public class TeacherModel
    {
       
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        //public List<GroupModel> Groups { get; set; }

    }
}

