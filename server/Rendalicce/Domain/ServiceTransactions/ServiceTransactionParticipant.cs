﻿using Rendalicce.Domain.Users;
using ServiceProvider = Rendalicce.Domain.ServiceProviders.ServiceProvider;

namespace Rendalicce.Domain.ServiceTransactions;

public sealed class ServiceTransactionParticipant : Entity
{
    private ServiceTransactionParticipant() { }

    public bool Approved { get; private set; }
    public int? Credits { get; init; }
    public ServiceProviderInformation? ProvidedService { get; init; }
    public User User { get; init; } = null!;
    
    public static ServiceTransactionParticipant Initialize(int? credits, ServiceProvider? serviceProvider, User user, bool approved)
    {
        return new ServiceTransactionParticipant
        {
            Credits = credits,
            ProvidedService = serviceProvider is null ? null : new ServiceProviderInformation(serviceProvider),
            User = user,
            Approved = approved
        };
    }

    public void Approve()
    {
        Approved = true;
    }
    
    public void Complete(List<ServiceTransactionParticipant> participants)
    {
        var otherParticipant = participants.FirstOrDefault(p => p.User.Id != User.Id);
        
        if(Credits.HasValue)
            User.DeductCredits(Credits.Value);
        
        if(otherParticipant!.Credits.HasValue)
            User.AddCredits(otherParticipant.Credits.Value);
    }
}

public sealed class ServiceProviderInformation
{
    public ServiceProviderInformation(ServiceProvider serviceProvider)
    {
        Id = serviceProvider.Id;
        Name = serviceProvider.Name;
        Description = serviceProvider.Description;
        Category = serviceProvider.Category;
        Email = serviceProvider.Email;
        PhoneNumber = serviceProvider.PhoneNumber;
        CompanyName = serviceProvider.CompanyName;
    }
    
    private ServiceProviderInformation() { }
    
    public Guid Id { get; set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? CompanyName { get; set; }
}