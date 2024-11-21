using FplStatsWasm.Server.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using Xunit.Abstractions;

namespace FplStatsWasm.Server.Tests;

public class GetFplDataServiceTests
{
    private readonly ITestOutputHelper output;

    public GetFplDataServiceTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public async void GetPlayersData_happy_path()
    {
        //arrange
        var logger = new FakeLogger<GetFplDataService>();

        var hcFactory = new Mock<IHttpClientFactory>();
        hcFactory.Setup(hcf => hcf.CreateClient(string.Empty))
                 .Returns(new HttpClient());

        var service = new GetFplDataService(logger, hcFactory.Object);
    
        //act
        var players = await service.GetPlayersData();


        //assert
        Assert.NotEmpty(players);
        Assert.NotNull(logger.Collector.LatestRecord);
        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Information, logger.Collector.GetSnapshot()[0].Level);
        Assert.StartsWith("Count=", logger.Collector.LatestRecord.Message);
        
        output.WriteLine(logger.Collector.LatestRecord.Message);
    }
}