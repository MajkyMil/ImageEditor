using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MigrationHandler.Models
{
    public class MigrationHandlerDbContext : DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }

        public string DbPath { get; }

        public MigrationHandlerDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "migrationhandler.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite($"Data Source={DbPath}");
    }
  
}
