
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using FplStatsWasm.Shared.Models;


namespace FplStatsWasm.Client.Services;

public class GetLocalDataService
{
    public List<Player> PlayerList { get; private set; } = new List<Player>();

    public TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();

    private HttpClient httpClient;
    
    public GetLocalDataService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("backend");
        GetData();
    }

    private async void GetData()
    {
        /*PlayerList.Add(new Player(){
            FirstName="Gabriel", Minutes=112, Position=4, Price=68, SecondName="Fernando de Jesus", SelectedByPercent="0.9", TeamCode=3, TotalPoints=5
        });*/
        
        string url = "/Players";
        
        List<Player>? players = await httpClient.GetFromJsonAsync<List<Player>>(url);

        if(players!=null && players.Count>0){
            PlayerList = players;
            taskCompletionSource.SetResult(1);
        }
    }
}
