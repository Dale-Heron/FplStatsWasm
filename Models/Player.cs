using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

public class Player
{
    [JsonPropertyName("first_name")]
    public required string FirstName { get; set; }

    [JsonPropertyName("second_name")]
    public required string SecondName { get; set; }

    [JsonPropertyName("element_type")]
    public required int Position { get; set; }

    [JsonPropertyName("team_code")]
    public required int TeamCode { get; set; }

    [JsonPropertyName("total_points")]
    public required int TotalPoints { get; set; }

    [JsonPropertyName("now_cost")]
    public required int Price { get; set; }

    [JsonPropertyName("minutes")]
    public required int Minutes { get; set; }

    [JsonPropertyName("selected_by_percent")]
    public required string SelectedByPercent { get; set; }

    public float PriceF => Price/10.0f;

    public float SelectedByPercentF => float.Parse(SelectedByPercent);

    public float PointsOverPrice => TotalPoints*1.0f / Price;

    public string Team => teams[TeamCode];

    static Dictionary<int, string> teams = new Dictionary<int, string>()
    {
        {3,     "ARS"},
        {7,     "AST"},
        {91,    "BOU"},
        {94,    "BRE"},
        {36,    "BRI"},
        {8,     "CHE"},
        {31,    "CRY"},
        {11,    "EVE"},
        {54,    "FUL"},
        {40,    "IPS"},
        {13,    "LEI"},
        {14,    "LIV"},
        {43,    "MCI"},
        {1,     "MAN"},
        {4,     "NEW"},
        {17,    "NOT"},
        {20,    "SOU"},
        {6,     "TOT"},
        {21,    "WHM"},
        {39,    "WOL"}
    };
}