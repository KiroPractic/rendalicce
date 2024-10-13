using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.ServiceTransactions;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceTransactions;

public sealed class GetServiceTransactions
{
    public sealed record GetServiceTransactionsResponse(IEnumerable<ServiceTransaction> ServiceTransactions);

    public sealed class GetServiceTransactionsEndpoint : EndpointWithoutRequest<GetServiceTransactionsResponse>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Get("service-transactions");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var authenticatedUser = HttpContext.GetAuthenticatedUser();
            var transactions = await DbContext.ServiceTransactions
                .Include(st => st.Participants)
                .ThenInclude(p => p.User)
                .Where(st => st.Participants.Any(p => p.User.Id == authenticatedUser.Id))
                .OrderByDescending(st => st.CreatedOn)
                .ToListAsync(ct);
            
            await SendAsync(new(transactions), cancellation: ct);
        }
    }
}