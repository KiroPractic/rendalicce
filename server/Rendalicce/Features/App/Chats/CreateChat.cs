// using FastEndpoints;
// using Rendalicce.Persistency;
//
// namespace Rendalicce.Features.App.Chats;
//
// public sealed class GetChatWithUser
// {
//     public sealed record GetChatWithUserRequest(Guid UserId);
//
//     public sealed record GetChatWithUserResponse();
//
//     public sealed class GetChatWithUserEndpoint : Endpoint<GetChatWithUserRequest, Result<GetChatWithUserResponse>>
//     {
//         public required DatabaseContext DbContext { get; init; }
//
//         public override void Configure()
//         {
//             Get("chats/users/{userId}");
//         }
//
//         public override async Task HandleAsync(GetChatWithUserRequest request, CancellationToken ct)
//         {
//             await SendAsync(new(new()), cancellation: ct);
//         }
//     }
//
//     public sealed class GetChatWithUserValidator : Validator<GetChatWithUserRequest>
//     {
//         public GetChatWithUserValidator()
//         {
//         }
//     }
// }