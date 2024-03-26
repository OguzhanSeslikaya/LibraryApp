using Loan.API.Contexts;
using Loan.API.Services.Abstractions;
using Loan.Shared.Entities.Models;
using Loan.Shared.Entities.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Shared.Events.Loan;
using System.Text.Json;

namespace Loan.API.Services.Concretes
{
    public class LoanService(LoanAPIDbContext context) : ILoanService
    {
        public async Task<BaseResponse> createLoan(string? userId,string? username,CreateLoanVM createLoanVM)
        {
            if (userId == null || username == null)
                return new BaseResponse() { succeeded=false,message="Kullanıcı bulunamadı."};

            Loan.Shared.Entities.Models.Loan loan = new()
            {
                id = Guid.NewGuid().ToString(),
                bookId = createLoanVM.bookId,
                loanDate = DateTime.UtcNow,
                state = RabbitMQ.Shared.Enums.LoanStateEnum.pending,
                userName = username,
                userId = userId
            };

            await context.loans.AddAsync(loan);

            var idempotentToken = Guid.NewGuid().ToString();

            LoanCreatedEvent loanCreatedEvent = new LoanCreatedEvent()
            {
                loanId = loan.id,
                bookId = createLoanVM.bookId,
                idempotentToken = idempotentToken,
                typeName = nameof(LoanCreatedEvent)
            };

            LoanOutbox loanOutbox = loanOutboxBuilder(idempotentToken,loanCreatedEvent,nameof(LoanCreatedEvent));

            await context.loanOutboxes.AddAsync(loanOutbox);
            await context.SaveChangesAsync();

            return new BaseResponse() { succeeded = true, message = "Ödünç alma talebi oluşturuldu." };
        }

        public async Task<List<LoanVM>> getLoans()
        {
            return await context.loans
                .Select(a => new LoanVM()
                {
                    bookId = a.bookId,
                    id = a.id,
                    loanDate = a.loanDate,
                    returnDate = a.returnDate,
                    state = Enum.GetName(a.state),
                    userId = a.userId,
                    username = a.userName
                })
                .OrderByDescending(a => a.loanDate)
                .ToListAsync();
        }

        public async Task<List<LoanVM>> getLoansByUsername(string? username)
        {
            if (username == null)
                return await getLoans();
            return await context.loans
                .Where(a => a.userName == username)
                .Select(a => new LoanVM()
                {
                    bookId = a.bookId,
                    id = a.id,
                    loanDate = a.loanDate,
                    returnDate = a.returnDate,
                    state = Enum.GetName(a.state),
                    userId = a.userId,
                    username = a.userName
                })
                .OrderByDescending(a => a.loanDate)
                .ToListAsync();
        }

        public async Task<BaseResponse> giveBack(string loanId)
        {
            var loan = await context.loans.Where(a => a.id == loanId).FirstOrDefaultAsync();
            if (loan != null)
            {
                loan.state = RabbitMQ.Shared.Enums.LoanStateEnum.returnedSuccesfully;
                loan.returnDate = DateTime.UtcNow;

                var idempotentToken = Guid.NewGuid().ToString();

                ReturnLoanEvent returnLoanEvent = new ReturnLoanEvent()
                {
                    idempotentToken = idempotentToken,
                    stockId = loan.bookId,
                    typeName = nameof(ReturnLoanEvent)
                };

                LoanOutbox loanOutbox = loanOutboxBuilder(idempotentToken,returnLoanEvent,nameof(ReturnLoanEvent));

                await context.loanOutboxes.AddAsync(loanOutbox);
                await context.SaveChangesAsync();

                return new BaseResponse() { succeeded = true, message = "Kitabı geri verme işlemi başarı ile gerçekleşti." };
            }
            return new BaseResponse() { succeeded = false, message = "Kitabı geri verme işlemi gerçekleştirilemedi."};
        }





        private LoanOutbox loanOutboxBuilder(string idempotentToken, object @event, string type)
        {
            return new LoanOutbox()
            {
                idempotentToken = idempotentToken,
                occuredOn = DateTime.UtcNow,
                payload = JsonSerializer.Serialize(@event),
                type = type
            };
        }

        private LoanVM loanVMBuilder(Shared.Entities.Models.Loan loan)
        {
            return new LoanVM() 
            {
                bookId = loan.bookId,
                id = loan.id,
                loanDate = loan.loanDate,
                returnDate = loan.returnDate,
                state = Enum.GetName(loan.state),
                userId = loan.userId,
                username = loan.userName
            };
        }
    }
}
