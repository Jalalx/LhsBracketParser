using System;

namespace LhsBracketParser.LinqProviderIntegrationTests.Internals
{
    public class PeopleDbContextFixture : IDisposable
    {
        public PeopleDbContext DbContext { get; }

        public PeopleDbContextFixture()
        {
            DbContext = new PeopleDbContext();
        }

        public virtual void Dispose()
        {
            DbContext.Database.Delete();
            DbContext.Dispose();
        }
    }
}
