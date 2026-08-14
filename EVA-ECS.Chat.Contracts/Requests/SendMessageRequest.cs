namespace EVA_ECS.Chat.Contracts.Requests;

/// <summary>
/// Frontend sendet eine neue Nachricht an das Gateway (/api/chat).
/// Das Gateway ergänzt die SenderId aus dem Token und macht daraus das ChatMessagePublishedEvent.
/// </summary>
public record SendMessageRequest
{
    public Guid RoomId { get; init; }
    public string EncryptedPayload { get; init; } = string.Empty;
    public string Signature { get; init; } = string.Empty;
}

/// <summary>
/// Frontend erstellt einen neuen Chat-Raum (1-zu-1 oder Gruppe) (/api/rooms).
/// </summary>
public record CreateRoomRequest
{
    public string? RoomName { get; init; }
    public bool IsGroupChat { get; init; }

    // Key = UserId des Teilnehmers
    // Value = Der symmetrische AES-Raum-Schlüssel, verschlüsselt mit dem Public Key des jeweiligen Users
    public Dictionary<Guid, string> EncryptedRoomKeys { get; init; } = new();
}