using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;

namespace Rendalicce.Features.Administration.Users;

public sealed class GetUsers
{
    public sealed record GetUsersResult(IEnumerable<GetUsersResult.User> Users)
    {
        public sealed record User(string FirstName, string LastName, string Email);
    }

    public sealed class GetUsersEndpoint : EndpointWithoutRequest<GetUsersResult>
    {
        private readonly DatabaseContext _dbContext;

        public GetUsersEndpoint(DatabaseContext databaseContext, EmailSendingService emailSendingService) => _dbContext = databaseContext;

        public override void Configure()
        {
            Get("administration/users");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var users = await _dbContext.Users.Select(u => new GetUsersResult.User(
                    u.FirstName,
                    u.LastName,
                    u.Email))
                .ToListAsync(ct);
            
            await SendAsync(new GetUsersResult(users), cancellation: ct);
        }
    }
}