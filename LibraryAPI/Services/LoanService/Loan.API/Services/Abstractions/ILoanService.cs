using Loan.Shared.Entities.Models;
using Loan.Shared.Entities.ViewModels;

namespace Loan.API.Services.Abstractions
{
    public interface ILoanService
    {
        public Task<BaseResponse> giveBack(string loanId);
        public Task<List<LoanVM>> getLoansByUsername(string? username);
        public Task<List<LoanVM>> getLoans();
        public Task<BaseResponse> createLoan(string? userId, string? username,CreateLoanVM createLoanVM);
    }
}
