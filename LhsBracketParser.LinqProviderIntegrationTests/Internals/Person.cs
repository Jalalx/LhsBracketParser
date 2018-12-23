using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace LhsBracketParser.LinqProviderIntegrationTests.Internals
{

    public class Person
    {
        public Person()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string FullName { get; set; }

        public int Age { get; set; }
    }
}
