namespace EVA_ECS.Chat.Contracts.Responses;

/// <summary>
/// Antwort auf eine Nutzersuche im UserService (/api/users/search?name=...).
/// </summary>
public record UserSearchResponse
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;

    // E2EE: Zwingend erforderlich, damit der Suchende diesem Nutzer eine Nachricht/einen Raumschlüssel verschlüsseln kann
    public string PublicKey { get; init; } = string.Empty;

    // Präsenz-Status
    public bool IsOnline { get; init; }
    public DateTimeOffset? LastSeen { get; init; }
}

/// <summary>
/// Detaillierte Informationen zu einem Chat-Raum (/api/rooms/{id}).
/// </summary>
public record RoomDetailResponse
{
    public Guid RoomId { get; init; }
    public string? RoomName { get; init; }
    public bool IsGroupChat { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    // Liste der Teilnehmer inkl. ihrer verschlüsselten Schlüssel für diesen Raum
    public List<RoomParticipantDto> Participants { get; init; } = new();
}

/// <summary>
/// Hilfs-DTO für die Raum-Teilnehmer.
/// </summary>
public record RoomParticipantDto
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;

    // E2EE: Nur der Nutzer, dem diese UserId gehört, kann diesen String mit seinem Private Key entschlüsseln
    public string EncryptedRoomKey { get; init; } = string.Empty;
}

/// <summary>
/// Die Historie einer Nachricht, wenn das Frontend alte Chats lädt (/api/rooms/{id}/messages).
/// </summary>
public record MessageHistoryResponse
{
    public Guid MessageId { get; init; }
    public Guid SenderId { get; init; }
    public string EncryptedPayload { get; init; } = string.Empty;
    public string Signature { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; }
}