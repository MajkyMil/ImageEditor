using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrationHandler.Models;

namespace MigrationHandler.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student");
            builder.Property(s => s.Age)
                .IsRequired(true);

            builder.HasData
            (
                new Student
                {
                    Id = 1,
                    Name = "John Doe",
                    Age = 30
                },
                new Student
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Age = 25
                },
                new Student
                {
                    Id = 3,
                    Name = "Mike Miles",
                    Age = 28
                }
            );
        }
    }
}
