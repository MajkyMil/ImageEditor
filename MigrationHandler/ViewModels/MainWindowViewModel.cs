using Microsoft.EntityFrameworkCore;
using MigrationHandler.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationHandler.ViewModels
{
    public class MainWindowViewModel
    {
        private  MigrationHandlerDbContext _dbContext { get;  }

        public ObservableCollection<Blog> Blogs { get; set; }
        public ObservableCollection<string> Migrations { get; set; }

        public MainWindowViewModel(MigrationHandlerDbContext dbContext)
        {
            _dbContext= dbContext;
            InitWindow();
        }

        public void InitWindow()
        {
            InitTestData();
            Blogs = new ObservableCollection<Blog>(_dbContext.Blogs);
            Migrations = new ObservableCollection<string>(_dbContext.Database.GetMigrations());
        }

        public void InitTestData()
        {
            //_dbContext.Blogs.Add(new Blog() { Url = "Test url"});
            //_dbContext.Blogs.Add(new Blog() { Url = "Url 1" });
            //_dbContext.Blogs.Add(new Blog() { Url = "Url 2" });
            //_dbContext.SaveChanges();
        }
    }
}
