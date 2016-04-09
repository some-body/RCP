﻿using Domain.Entities;
using System.Data.Entity;

namespace Domain
{
    public class CoursesContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
