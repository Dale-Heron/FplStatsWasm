using FplStatsWasm.Server.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using Xunit.Abstractions;

namespace FplStatsWasm.Server.Tests;

public class GetFplDataServiceTests
{
    private readonly ITestOutputHelper _output;

    public GetFplDataServiceTests(ITestOutputHelper output)
    {
        this._output = output;
    }

    [Fact]
    public async void GetPlayersData_happy_path()
    {
        //arrange
        var service = GetService(out var logger);
    
        //act
        var players = await service.GetPlayersData();


        //assert
        Assert.NotEmpty(players);
        Assert.NotNull(logger.Collector.LatestRecord);
        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Information, logger.Collector.GetSnapshot()[0].Level);
        Assert.StartsWith("Found ", logger.Collector.LatestRecord.Message);
        
        _output.WriteLine(logger.Collector.LatestRecord.Message);
    }
    
    [Fact]
    public async void GetPlayersData_Call_twice_should_cache()
    {
        //arrange
        var service = GetService(out var logger);

        //act
        await service.GetPlayersData();
        
        var players = await service.GetPlayersData();


        //assert
        Assert.NotEmpty(players);
        Assert.NotNull(logger.Collector.LatestRecord);
        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Information, logger.Collector.GetSnapshot()[0].Level);
        Assert.StartsWith("Found ", logger.Collector.LatestRecord.Message);
        
        _output.WriteLine(logger.Collector.LatestRecord.Message);
    }

    private static GetFplDataService GetService(out FakeLogger<GetFplDataService> logger)
    {
        logger = new FakeLogger<GetFplDataService>();

        var hcFactory = new Mock<IHttpClientFactory>();
        hcFactory.Setup(hcf => hcf.CreateClient(string.Empty))
            .Returns(new HttpClient());
        
        var inmemory = new MemoryCache(new MemoryCacheOptions());

        var service = new GetFplDataService(inmemory, logger, hcFactory.Object);
        return service;
    }
}