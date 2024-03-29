using Amazon.DynamoDBv2;
using FluentAssertions;
using Villains.Library.Aws.Tests.TestFixtures;
using Villains.Library.Services;

namespace Villains.Library.Aws.Tests;

public class VillainsServiceTests : IClassFixture<EnvironmentFixture>
{
    public VillainsServiceTests(EnvironmentFixture fixture)
    {

    }

    [Fact]
    public async Task GetAsync_ValidId_VillainReturned()
    {
        // arrange
        var service = new VillainsService(new AmazonDynamoDBClient());

        // act
        var result = await service.GetAsync("61635bf81b24a6aad5f3756a");
        var villain = result.Value;

        // assert
        result.IsSuccess.Should().BeTrue();
        villain.Should().NotBeNull();
        villain.Name.Should().NotBeNullOrWhiteSpace();
        villain.Powers.Should().NotBeNullOrWhiteSpace();
        villain.Id.Should().NotBeNullOrWhiteSpace();
        villain.MimeType.Should().NotBeNullOrWhiteSpace();
        villain.ButtonText.Should().NotBeNullOrWhiteSpace();
        villain.Saying.Should().NotBeNullOrWhiteSpace();
    }
}