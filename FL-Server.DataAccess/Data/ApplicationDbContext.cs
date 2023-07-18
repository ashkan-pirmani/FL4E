using FL_Server.Models;
using FL_Server.Models.Models;
using FL_Server.Models.Models.DataModel;
using FL_Server.Models.Models.DataModel.DataCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FL_Server.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<StudyCenter> StudyCenter { get; set; }
        public DbSet<ModelCenter> ModelCenter { get; set; }

        public DbSet<FileOnDatabaseModel> FilesOnDatabase { get; set; }

        public DbSet<DataDictionaryOnDatabaseModel> DataDictionaryOnDatabase { get; set; }
        public DbSet<DataDictionaryOnFileSystemModel> DataDictionaryOnFileSystem { get; set; }

        public DbSet<DataCleanFunctionOnDatabaseModel> DataCleanFunctionOnDatabase { get; set; }
        public DbSet<DataCleanFunctionOnFileSystemModel> DataCleanFunctionOnFileSystem { get; set; }

        public DbSet<DataInputModelOnDatabaseModel> DataInputModelOnDatabase { get; set; }
        public DbSet<DataInputModelOnFileSystemModel> DataInputModelOnFileSystem { get; set; }

        public DbSet<DataCoreFileOnSystemModel> DataCoreFileOnSystem { get; set; }
        public DbSet<DataCoreFileOnDatabaseModel> DataCoreFileOnDatabase { get; set; }



        public DbSet<StudyCatalogue> StudyCatalogue { get; set; }
        public DbSet<ModelCatalogue> ModelCatalogue { get; set; }
        public DbSet<AnalysisCenter> AnalysisCenter { get; set; }

    }
}