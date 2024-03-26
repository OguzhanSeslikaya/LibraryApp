using Loan.API.Contexts;
using Loan.API.Services.Abstractions;
using Loan.Shared.Entities.Models;
using Loan.Shared.Entities.ViewModels;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Shared.Events.Loan;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Loan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoanController(ILoanService loanService) : ControllerBase
    {
        [HttpGet("getMyLoans")]
        public async Task<IActionResult> getMyLoans()
        {
            return Ok(await loanService.getLoansByUsername(User.Claims.FirstOrDefault(a => a.Type == "userName")?.Value));
        }

        [HttpGet("admin/getLoans")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> getLoans()
        {
            return Ok(await loanService.getLoans());
        }

        [HttpGet("admin/getLoansByUsername")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> getLoansByUsername([FromQuery] string? username)
        {
            return Ok(await loanService.getLoansByUsername(username));
        }

        [HttpPost("createLoan")]
        public async Task<IActionResult> createLoan([FromBody] CreateLoanVM createLoan)
        {
            return Ok(await loanService.createLoan(
                User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value,
                User.Claims.FirstOrDefault(a => a.Type == "userName")?.Value,
                createLoan
                ));
        }

        [HttpPost("admin/giveBack")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> giveBack([FromQuery] string loanId)
        {
            return Ok(await loanService.giveBack(loanId));
        }
    }
}
