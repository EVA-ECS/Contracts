namespace Chat.Contracts.Events;

public record ChatMessageEvent(
    string MessageId,
    string SenderId,
    string TargetId,
    string Ciphertext,
    DateTime Timestamp
);