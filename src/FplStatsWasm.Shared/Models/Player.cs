using System.Text.Json.Serialization;

namespace FplStatsWasm.Shared.Models;

public class Player
{
    [JsonPropertyName("first_name")]
    public required string FirstName { get; init; }

    [JsonPropertyName("second_name")]
    public required string SecondName { get; init; }

    [JsonPropertyName("element_type")]
    public required int Position { get; init; }

    [JsonPropertyName("team_code")]
    public required int TeamCode { get; init; }

    [JsonPropertyName("total_points")]
    public required int TotalPoints { get; init; }

    [JsonPropertyName("now_cost")]
    public required int Price { get; init; }

    [JsonPropertyName("minutes")]
    public required int Minutes { get; init; }

    [JsonPropertyName("selected_by_percent")]
    public required string SelectedByPercent { get; init; }

    public float PriceF => Price/10.0f;

    public float SelectedByPercentF => float.Parse(SelectedByPercent);

    public float PointsOverPrice => TotalPoints*1.0f / Price;

    public string Team => Teams[TeamCode];

    private static readonly Dictionary<int, string> Teams = new Dictionary<int, string>()
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