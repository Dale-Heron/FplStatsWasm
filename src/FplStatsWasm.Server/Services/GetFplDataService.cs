using System.Text.Json;
using FplStatsWasm.Shared.Models;
using Microsoft.Extensions.Caching.Memory;


namespace FplStatsWasm.Server.Services;

public class GetFplDataService
{
    private readonly HttpClient _httpClient;
    
    private readonly IMemoryCache _memoryCache;

    private readonly ILogger<GetFplDataService> _logger;
    
    public GetFplDataService(IMemoryCache memoryCache,
        ILogger<GetFplDataService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }
    
    public async Task<List<Player>> GetPlayersData()
    {
        return (await _memoryCache.GetOrCreateAsync(
            $"{this.GetType().Name}.GetPlayersData",
            _ => GetPlayersDataOverWire() ))!;

        async Task<List<Player>> GetPlayersDataOverWire()
        {
            var players = new List<Player>();
            string url = "https://fantasy.premierleague.com/api/bootstrap-static/";
        
            var topLevel = await _httpClient.GetFromJsonAsync<Dictionary<string, JsonElement>>(url);

            if( topLevel != null)
            {
                var tempPlayers = JsonSerializer.Deserialize<List<Player>>(topLevel["elements"]);

                if(tempPlayers != null)
                {
                    _logger.LogInformation($"Found {tempPlayers.Count} players");
                    players = tempPlayers;
                }
            }

            return players;
        }
    }
}
