using System.Net.Http.Json;
using FplStatsWasm.Shared.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FplStatsWasm.Client.Services;

public class GetLocalDataService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GetLocalDataService> _logger;

    public GetLocalDataService(IMemoryCache memoryCache,
        ILogger<GetLocalDataService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("backend");
    }
    
    public async Task<List<Player>> GetPlayers()
    {
        return (await _memoryCache.GetOrCreateAsync(
            $"{this.GetType().Name}.GetPlayers",
            _ => GetPlayersDataFromServer() ))!;

        async Task<List<Player>> GetPlayersDataFromServer()
        {
            const string url = "/Players";
        
            var players = await _httpClient.GetFromJsonAsync<List<Player>>(url);

            if(players!=null && players.Count>0)
            {
                _logger.LogInformation($"Found {players.Count} players");
                return players;
            }

            return new List<Player>();
        }
    }
}
