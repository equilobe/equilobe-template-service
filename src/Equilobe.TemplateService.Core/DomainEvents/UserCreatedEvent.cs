using MediatR;

namespace Equilobe.TemplateService.Core.DomainEvents;

public record UserCreatedEvent : INotification
{
    public UserCreatedEvent(
        long id, 
        string email, 
        string firstName, 
        string lastName, 
        string externalId, 
        string? profilePictureUrl)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        ExternalId = externalId;
        ProfilePictureUrl = profilePictureUrl;
    }

    public long Id { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string ExternalId { get; init; }
    public string? ProfilePictureUrl { get; init; }
}