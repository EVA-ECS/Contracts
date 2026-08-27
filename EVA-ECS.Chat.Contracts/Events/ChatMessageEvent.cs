namespace Chat.Contracts.Events;

[Obsolete("Use EVA_ECS.Chat.Contracts.Events.ChatMessagePublishedEvent (contract v2).")]
public record ChatMessageEvent(
    string MessageId,
    string SenderId,
    string TargetId,
    string Ciphertext,
    DateTime Timestamp
);
