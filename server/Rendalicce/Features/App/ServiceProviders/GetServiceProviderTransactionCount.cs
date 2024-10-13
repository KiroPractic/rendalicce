using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Reviews;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;
using ServiceProvider = Rendalicce.Domain.ServiceProviders.ServiceProvider;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class GetServiceProviderTransactionCount
{
    public sealed record GetServiceProviderTransactionCountRequest(Guid Id);
    public sealed record GetServiceProviderTransactionCountResult(int TransactionCount);

    public sealed class GetServiceProviderTransactionCountEndpoint : Endpoint<GetServiceProviderTransactionCountRequest, GetServiceProviderTransactionCountResult>
    {
        private readonly DatabaseContext _dbContext;

        public GetServiceProviderTransactionCountEndpoint(DatabaseContext databaseContext, EmailSendingService emailSendingService) => _dbContext = databaseContext;

        public override void Configure()
        {
            AllowAnonymous();
            Get("service-providers/{id}/transaction-count");
        }

        public override async Task HandleAsync(GetServiceProviderTransactionCountRequest req, CancellationToken ct)
        {
            var transactionsCount = await _dbContext.ServiceTransactions
                .Where(st => st.Completed && st.Participants.Any(p => p.ProvidedService != null && p.ProvidedService.Id == req.Id))
                .CountAsync(ct);
           
            await SendAsync(new GetServiceProviderTransactionCountResult(transactionsCount), cancellation: ct);
        }
    }
}