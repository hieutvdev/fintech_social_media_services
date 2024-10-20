using BuildingBlocks.Messaging.Messaging.Events;

namespace BuildingBlocks.Messaging.Messaging.MassTransit.MassTransitEvents;


#nullable disable
public record ConfirmAccountEvents : IntegrationEvent
{
    public string Host { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }
}