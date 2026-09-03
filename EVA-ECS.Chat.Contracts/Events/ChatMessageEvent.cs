namespace Chat.Contracts.Events;

/// <summary>
/// Plaintext private-message event used by the current MVP.
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
