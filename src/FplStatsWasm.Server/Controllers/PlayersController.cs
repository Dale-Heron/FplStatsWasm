using FplStatsWasm.Server.Services;
using FplStatsWasm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace FplStatsWasm.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    private readonly GetFplDataService _getFplDataService;

    public PlayersController(ILogger<PlayersController> logger, GetFplDataService getFplDataService)
    {
        _logger = logger;
        _getFplDataService = getFplDataService;
    }

    [HttpGet(Name = "GetPlayer")]
    public async Task<IEnumerable<Player>> Get()
    {
        _logger.LogInformation("Get players");
        var list = await _getFplDataService.GetPlayersData();

        _logger.LogInformation($"Count={list.Count}");

        return list;
    }
}
