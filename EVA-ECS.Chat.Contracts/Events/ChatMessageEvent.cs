namespace Chat.Contracts.Events;

/// <summary>
/// Private-message event used by the current MVP. Ciphertext contains the
/// serialized encrypted browser envelope; backend services do not decrypt it.
/// TargetId is the recipient user ID. Storage resolves the private room from
/// the sender and recipient before inserting the message into Supabase.
/// </summary>
public record ChatMessageEvent(
    string MessageId,
    string SenderId,
    string TargetId,
    string Ciphertext,
    DateTime Timestamp
);
