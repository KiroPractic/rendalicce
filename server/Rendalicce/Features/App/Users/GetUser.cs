using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.ServiceSeekers;
using Rendalicce.Domain.Users;
using Rendalicce.Persistency;
using ServiceProvider = Rendalicce.Domain.ServiceProviders.ServiceProvider;

namespace Rendalicce.Features.App.Users;

public sealed class GetUser
{
    public sealed record GetUserRequest(Guid Id);

    public sealed record GetUserResponse(User User, IEnumerable<ServiceProvider> ServiceProviders, IEnumerable<ServiceSeeker> ServiceSeekers);

    public sealed class GetUserEndpoint : Endpoint<GetUserRequest, GetUserResponse>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Get("users/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == req.Id, ct);
            if(user is null)
                ThrowError("Entitet ne postoji.");
            
            var serviceProviders = await DbContext.ServiceProviders.Where(sp => sp.Owner.Id == req.Id).ToListAsync(ct);
            var serviceSeekers = await DbContext.ServiceSeekers.Where(sp => sp.Owner.Id == req.Id).ToListAsync(ct);
            await SendAsync(new(user, serviceProviders, serviceSeekers), cancellation: ct);
        }
    }
}