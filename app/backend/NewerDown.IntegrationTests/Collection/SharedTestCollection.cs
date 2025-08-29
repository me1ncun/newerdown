namespace NewerDown.IntegrationTests.Collection;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<CustomWebApplicationFactory>;