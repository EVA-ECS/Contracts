using EVA_ECS.Chat.Contracts.Messages;

namespace EVA_ECS.Chat.Contracts.Events;

/// <summary>
/// Wird ausgelöst, wenn ein Nutzer eine Nachricht sendet.
/// Wird vom StorageService (für die DB) und DeliveryService (für WebSockets) gelesen.
/// </summary>
public record ChatMessagePublishedEvent
{
    public Guid MessageId { get; init; }
    public Guid SenderId { get; init; }
    public Guid TargetId { get; init; }
    public long Timestamp { get; init; }
    public EncryptedMessagePayload Payload { get; init; } = new();
}

/// <summary>
/// Wird ausgelöst, wenn ein Nutzer online oder offline geht (via Gateway/Auth).
/// Wird vom UserService gelesen, um den Status in der DB/Redis zu aktualisieren.
/// </summary>
public record UserStatusChangedEvent
{
    public Guid UserId { get; init; }
    public bool IsOnline { get; init; }
    public DateTimeOffset Timestamp { get; init; }
}
