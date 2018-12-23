using System.Data.Entity;

namespace LhsBracketParser.LinqProviderIntegrationTests.Internals
{
    public class PeopleDbContext : DbContext
    {
#if DEBUG
        private const string ConnectionStringName = "default";
#else
        private const string ConnectionStringName = "appveyor";
#endif

        static PeopleDbContext()
        {
            Database.SetInitializer(new PeopleDbInitializer());
        }

        public PeopleDbContext() : base(ConnectionStringName)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(x => x.Id);
        }

        public DbSet<Person> People { get; set; }
    }
}
