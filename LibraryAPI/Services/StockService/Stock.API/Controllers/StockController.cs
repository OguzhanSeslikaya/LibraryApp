using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Stock.Shared.CQRS.Commands.CreateStock;
using Stock.Shared.CQRS.Commands.CurrentStockChange;
using Stock.Shared.CQRS.Commands.TotalStockChange;
using Stock.Shared.CQRS.Queries.GetStocks;
using Stock.Shared.CQRS.Queries.GetStocksForAdmin;
using Stock.Shared.Entities.ViewModels;
using Stock.Shared.Services.Abstractions;
using System.Text.Json;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockController(IMediator mediator) : ControllerBase
    {
        [HttpGet("getStocks")]
        public async Task<IActionResult> getStocks([FromQuery]GetStocksQueryRequest getStocksQueryRequest)
        {
            return Ok(await mediator.Send(getStocksQueryRequest));
        }

        [HttpGet("admin/getStocks")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> getStocksForAdmin([FromQuery]GetStocksForAdminQueryRequest getStocksForAdminQueryRequest)
        {
            return Ok(await mediator.Send(getStocksForAdminQueryRequest));
        }

        [HttpPost("admin/createStock")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> createStock([FromBody]CreateStockCommandRequest createStockCommandRequest)
        {
            return Ok(await mediator.Send(createStockCommandRequest));
        }

        [HttpPost("admin/totalStockChange")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> totalStockChange([FromBody] TotalStockChangeCommandRequest totalStockChangeCommandRequest)
        {
            return Ok(await mediator.Send(totalStockChangeCommandRequest));
        }

        [HttpPost("admin/currentStockChange")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> currentStockChange([FromBody] CurrentStockChangeCommandRequest currentStockChangeCommandRequest)
        {
            return Ok(await mediator.Send(currentStockChangeCommandRequest));
        }
    }
}
