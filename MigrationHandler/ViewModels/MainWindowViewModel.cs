using Microsoft.EntityFrameworkCore;
using MigrationHandler.Models;
using System.Collections.ObjectModel;

namespace MigrationHandler.ViewModels
{
    public class MainWindowViewModel
    {
        private MigrationHandlerDbContext _dbContext { get; }

        public ObservableCollection<string> Migrations { get; set; }
        public ObservableCollection<string> ApplieMigrations { get; set; }
        public ObservableCollection<string> PendingMigrations { get; set; }

        public MainWindowViewModel(MigrationHandlerDbContext dbContext)
        {
            _dbContext = dbContext;
            InitWindow();
        }

        public void InitWindow()
        {
            Migrations = new ObservableCollection<string>(_dbContext.Database.GetMigrations());
            ApplieMigrations = new ObservableCollection<string>(_dbContext.Database.GetAppliedMigrations());
            PendingMigrations = new ObservableCollection<string>(_dbContext.Database.GetPendingMigrations());
           
        }
    }
}
