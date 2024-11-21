
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using FplStatsWasm.Shared.Models;
using FplStatsWasm.Server.Controllers;


namespace FplStatsWasm.Server.Services;

public class GetFplDataService
{
    private HttpClient httpClient;

    private readonly ILogger<GetFplDataService> _logger;
    
    public GetFplDataService(ILogger<GetFplDataService> logger,IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        httpClient = httpClientFactory.CreateClient();
    }

    public async Task<List<Player>> GetPlayersData()
    {
        List<Player> players = new List<Player>();
        string url = "https://fantasy.premierleague.com/api/bootstrap-static/";
        
        var topLevel = await httpClient.GetFromJsonAsync<Dictionary<string, JsonElement>>(url);

        if( topLevel != null){
            var tempPlayers = JsonSerializer.Deserialize<List<Player>>(topLevel["elements"]);

            if(tempPlayers != null){
                _logger.LogInformation("Count={Count}", tempPlayers.Count);
                players = tempPlayers;
            }
        }

        return players;
    }
}
