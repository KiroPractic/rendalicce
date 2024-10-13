using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceTransactions;

public sealed class ApproveServiceTransaction
{
    public sealed record ApproveServiceTransactionRequest(Guid Id);
    public sealed record ApproveServiceTransactionResult(Guid Id, bool Completed);


    public sealed class ApproveServiceTransactionEndpoint : Endpoint<ApproveServiceTransactionRequest, ApproveServiceTransactionResult>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Post("service-transactions/{id}/approve");
        }

        public override async Task HandleAsync(ApproveServiceTransactionRequest request, CancellationToken ct)
        {
            var authenticatedUser = HttpContext.GetAuthenticatedUser();
            var transaction = await DbContext.ServiceTransactions
                .Include(st => st.Participants)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(st => st.Id == request.Id 
                && st.Participants.Any(p => p.User.Id == authenticatedUser.Id), ct);
            
            if(transaction is null)
                ThrowError("Entitet ne postoji.");
            
            transaction.Approve(authenticatedUser);
            
            if(transaction.Completed && transaction.Participants.Any(p => p.User.GetCreditsValance() < 0))
                ThrowError("Korisnicic nemaju dovoljno resursa.");
            
            DbContext.ServiceTransactions.Update(transaction);
            await DbContext.SaveChangesAsync(ct);
            
            await SendAsync(new(transaction.Id, transaction.Completed), cancellation: ct);
        }
    }
}