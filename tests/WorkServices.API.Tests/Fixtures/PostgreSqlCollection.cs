using Xunit;

namespace WorkServices.API.Tests.Fixtures;

[CollectionDefinition("postgres")]
public class PostgreSqlCollection :
    ICollectionFixture<PostgreSqlFixture>
{
}