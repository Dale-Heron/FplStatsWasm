using System.Net.Http.Json;
using FplStatsWasm.Shared.Models;


namespace FplStatsWasm.Client.Services;

public class GetLocalDataService
{
    private HttpClient httpClient;
    
    public GetLocalDataService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("backend");
    }

    public async Task<List<Player>> GetPlayers()
    {
        string url = "/Players";
        
        List<Player>? players = await httpClient.GetFromJsonAsync<List<Player>>(url);

        if(players!=null && players.Count>0){
            return players;
        }

        return new List<Player>();
    }
}
