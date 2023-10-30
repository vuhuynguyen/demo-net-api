using Xunit;

namespace Application.IntegrationTests.Tests
{
    [CollectionDefinition(nameof(ApplicationCollection))]
    public class ApplicationCollection : ICollectionFixture<WebApiFixture>
    {
    }
}
