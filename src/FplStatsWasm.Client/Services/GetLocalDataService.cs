using System.Net.Http.Json;
using FplStatsWasm.Shared.Models;

namespace FplStatsWasm.Client.Services;

public class GetLocalDataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GetLocalDataService> _logger;

    public GetLocalDataService(ILogger<GetLocalDataService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("backend");
    }

    public async Task<List<Player>> GetPlayers()
    {
        string url = "/Players";
        
        List<Player>? players = await _httpClient.GetFromJsonAsync<List<Player>>(url);

        if(players!=null && players.Count>0){
            _logger.LogInformation($"Found {players.Count} players");
            return players;
        }

        return new List<Player>();
    }
}
