namespace EVA_ECS.Chat.Contracts.Events;

/// <summary>
/// Wird ausgelöst, wenn ein Nutzer eine Nachricht sendet.
/// Wird vom StorageService (für die DB) und DeliveryService (für WebSockets) gelesen.
/// </summary>
public record ChatMessagePublishedEvent
{
    public Guid MessageId { get; init; }
    public Guid RoomId { get; init; }
    public Guid SenderId { get; init; }

    // E2EE: Der eigentliche Text ist verschlüsselt. Der Server kann ihn nicht lesen!
    public string EncryptedPayload { get; init; } = string.Empty;

    // E2EE: Die Signatur des Senders (beweist, dass der Absender echt ist)
    public string Signature { get; init; } = string.Empty;

    public DateTimeOffset Timestamp { get; init; }
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