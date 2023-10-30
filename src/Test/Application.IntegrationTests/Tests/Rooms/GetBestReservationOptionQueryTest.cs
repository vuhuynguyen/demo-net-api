using ApplicationCore.Query.Features.Rooms;
using Xunit;

namespace Application.IntegrationTests.Tests.Rooms;

[Collection(nameof(ApplicationCollection))]
public class GetBestReservationOptionQueryTest
{
    private readonly WebApiFixture _fixture;
    public GetBestReservationOptionQueryTest(WebApiFixture fixture) => _fixture = fixture;
    
    [Theory]
    [InlineData(1, "Single - $30,00")]
    [InlineData(2, "Double - $50,00")]
    [InlineData(3, "Single Double - $80,00")]
    [InlineData(4, "Family - $85,00")]
    [InlineData(5, "Single Family - $115,00")]
    [InlineData(6, "Double Family - $135,00")]
    [InlineData(7, "Single Double Family - $165,00")]
    [InlineData(8, "Double Double Family - $185,00")]
    [InlineData(9, "Single Double Double Family - $215,00")]
    [InlineData(10, "Double Double Double Family - $235,00")]
    [InlineData(11, "Single Double Double Double Family - $265,00")]
    [InlineData(12, "Single Single Double Double Double Family - $295,00")]
    [InlineData(13, "No option")]
    public async Task GetBestReservationOptionQuery_ReturnsValidResult(int numberOfGuests, string expectedOption)
    {
        // Arrange: Prepare input data and query
        var query = new GetBestReservationOptionQuery { NumberOfGuests = numberOfGuests };

        // Act: Execute the query
        var result = await _fixture.SendMediatorAsync(FakePrincipals.Admin, query);

        // Assert
        Assert.NotNull(result); 
        Assert.Equal(expectedOption, result);
    }
    
    [Fact]
    public async Task GetBestReservationOptionQuery_NumberOfGuestsExceedsCapacity_ReturnNoOption()
    {
        // Arrange: Set the number of guests to a value greater than total capacity
        var random = new Random();
        var numberOfGuests = random.Next(13, 20);

        // Act
        var result = await _fixture.SendMediatorAsync(FakePrincipals.Admin, new GetBestReservationOptionQuery { NumberOfGuests = numberOfGuests });

        // Assert: Expect "No option" because the number of guests exceeds capacity
        Assert.Equal("No option", result);
    }

}