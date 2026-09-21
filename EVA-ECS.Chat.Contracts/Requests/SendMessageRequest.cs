using EVA_ECS.Chat.Contracts.Messages;

namespace EVA_ECS.Chat.Contracts.Requests;

/// <summary>
/// Frontend sendet eine neue, bereits verschlüsselte Nachricht an das Gateway.
/// SenderId kommt nicht vom Client, sondern wird aus dem validierten JWT gelesen.
/// </summary>
public record SendMessageRequest
{
    public Guid MessageId { get; init; }
    public Guid TargetId { get; init; }
    public long Timestamp { get; init; }
    public EncryptedMessagePayload Payload { get; init; } = new();
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
