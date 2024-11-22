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
    public async void GetPlayersData_Use_real_httpclient_and_memory_cache_Check_log_message_and_data()
    {
        //arrange
        var (service, logger) = GetFplDataService();
    
        //act
        var players = await service.GetPlayersData();

        //assert
        Assert.NotEmpty(players);

        CheckForSingleFoundLogMessage(logger);
        
        _output.WriteLine(logger.Collector.LatestRecord.Message);
    }
    
    [Fact]
    public async void GetPlayersData_Use_real_httpclient_and_memory_cache_Call_twice_should_cache()
    {
        //arrange
        var (service, logger) = GetFplDataService();

        //act
        await service.GetPlayersData();

        var players = await service.GetPlayersData();


        //assert
        Assert.NotEmpty(players);

        CheckForSingleFoundLogMessage(logger);

        _output.WriteLine(logger.Collector.LatestRecord.Message);
    }

    private static void CheckForSingleFoundLogMessage(FakeLogger<GetFplDataService> logger)
    {
        Assert.NotNull(logger.Collector.LatestRecord);
        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Information, logger.Collector.GetSnapshot()[0].Level);
        Assert.StartsWith("Found ", logger.Collector.LatestRecord.Message);
    }

    private static (GetFplDataService service, FakeLogger<GetFplDataService> logger) GetFplDataService()
    {
        var logger = new FakeLogger<GetFplDataService>();

        var hcFactory = new Mock<IHttpClientFactory>();
        hcFactory.Setup(hcf => hcf.CreateClient(string.Empty))
            .Returns(new HttpClient());
        
        var inmemory = new MemoryCache(new MemoryCacheOptions());

        var service = new GetFplDataService(inmemory, logger, hcFactory.Object);
        return (service, logger);
    }
}