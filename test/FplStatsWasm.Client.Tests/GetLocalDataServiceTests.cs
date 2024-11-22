using System.Text.Json;
using FplStatsWasm.Client.Services;
using FplStatsWasm.Shared.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using RichardSzalay.MockHttp;
using Xunit.Abstractions;

namespace FplStatsWasm.Client.Tests;

public class GetLocalDataServiceTests
{
    private readonly ITestOutputHelper _output;

    public GetLocalDataServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async void GetPlayersData_Use_fake_httpclient_and_real_memory_cache_Check_log_message_and_data()
    {
        //arrange
        var (service, logger) = GetLocalDataService();
    
        //act
        var players = await service.GetPlayers();

        //assert
        Assert.NotEmpty(players);

        CheckForSingleFoundLogMessage(logger);
    }
    
    [Fact]
    public async void GetPlayersData_Use_fake_httpclient_and_real_memory_cache_Call_twice_should_cache()
    {
        //arrange
        var (service, logger) = GetLocalDataService();

        //act
        await service.GetPlayers();
        
        var players = await service.GetPlayers();

        //assert
        Assert.NotEmpty(players);

        CheckForSingleFoundLogMessage(logger);
    }

    private void CheckForSingleFoundLogMessage(FakeLogger<GetLocalDataService> logger)
    {
        Assert.NotNull(logger.Collector.LatestRecord);
        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Information, logger.Collector.GetSnapshot()[0].Level);
        Assert.StartsWith("Found ", logger.Collector.LatestRecord.Message);

        _output.WriteLine(logger.Collector.LatestRecord.Message);
    }

    private static (GetLocalDataService, FakeLogger<GetLocalDataService>) GetLocalDataService()
    {
        var logger = new FakeLogger<GetLocalDataService>();
        
        var mockHttp = new MockHttpMessageHandler();

        var jsonString = JsonSerializer.Serialize(new List<Player>() { new Player(){FirstName = "bob", Minutes = 0, Position = 1, Price = 10, SecondName = "test", TeamCode = 1, TotalPoints = 2, SelectedByPercent = "75"}});

        // Setup a respond for the user api (including a wildcard in the URL)
        mockHttp.When("https://localhost/Players")
            .Respond("application/json", jsonString); // Respond with JSON

        var hc = mockHttp.ToHttpClient();
        hc.BaseAddress = new Uri("https://localhost");
        
        var hcFactory = new Mock<IHttpClientFactory>();
        hcFactory.Setup(hcf => hcf.CreateClient("backend"))
            .Returns(hc);
        
        var inmemory = new MemoryCache(new MemoryCacheOptions());

        var service = new GetLocalDataService(inmemory, logger, hcFactory.Object);
        return (service, logger);
    }
}