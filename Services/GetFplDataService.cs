
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FplStatsWasm
{
    public class GetFplDataService
    {
        public List<Player> PlayerList { get; private set; } = new List<Player>();

        public TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
    
        private HttpClient httpClient;
        
        public GetFplDataService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient();
            GetData();
        }

        private async void GetData()
        {
            PlayerList.Add(new Player(){
                FirstName="Gabriel", Minutes=112, Position=4, Price=68, SecondName="Fernando de Jesus", SelectedByPercent="0.9", TeamCode=3, TotalPoints=5
            });
            /*
            string url = "https://fantasy.premierleague.com/api/bootstrap-static/";
            
            var topLevel = await httpClient.GetFromJsonAsync<Dictionary<string, JsonElement>>(url);

            if( topLevel != null){
                List<Player>? players = JsonSerializer.Deserialize<List<Player>>(topLevel["elements"]);

                if(players != null){
                    PlayerList = players;
                }
            }*/

            taskCompletionSource.SetResult(1);
        }
    }
}