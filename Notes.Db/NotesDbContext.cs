using Microsoft.EntityFrameworkCore;
using Notes.Models.DbModels;

namespace Notes.Db
{
    public sealed class NotesDbContext : DbContext
    {
        public DbSet<Record> recordsTable { get; set; } = null!;
        public NotesDbContext(DbContextOptions opt) : base(opt)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder
            .Entity<Record>()
            .HasData(new Record()
            {
                id = 1,
                recordCreationTime = DateTime.Now,
                recordInfo = "someText",
                recordName = "first",
                recordType = "defaultNote"
            });
        //protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
        //{
        //    //if (!optBuilder.IsConfigured)
        //    //{
        //    //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //    //       .SetBasePath(Directory.GetCurrentDirectory())
        //    //       .AddJsonFile("appsettings.json")
        //    //       .Build();

            //    //    var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
            //    //    optBuilder.UseSqlServer(connectionString);
            //    //}

            //    ////IConfigurationRoot configuration = new ConfigurationBuilder()
            //    ////.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //    ////.AddJsonFile("appsettings.json")
            //    ////.Build();

            //    optBuilder.UseSqlServer(JsonConnectionBuilder
            //    .GetJsonConfigRoot()
            //    .GetConnectionString("DefaultConnection"));
            //}

        public IQueryable<Record> GetAllRecords() => recordsTable;
    }
}
