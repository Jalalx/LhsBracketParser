using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace LhsBracketParser.LinqProviderIntegrationTests.Internals
{

    public class PeopleDbInitializer : CreateDatabaseIfNotExists<PeopleDbContext>
    {
        protected override void Seed(PeopleDbContext context)
        {
            context.People.AddOrUpdate(x => x.FullName, new Person { FullName = "Will Smith", Age = 50 });
            context.People.AddOrUpdate(x => x.FullName, new Person { FullName = "John King", Age = 30 });
            context.People.AddOrUpdate(x => x.FullName, new Person { FullName = "Leo Messi", Age = 33 });
            context.People.AddOrUpdate(x => x.FullName, new Person { FullName = "Mark Jackson", Age = 45 });

            context.SaveChanges();
        }
    }
}
