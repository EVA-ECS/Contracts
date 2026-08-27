namespace EVA_ECS.Chat.Contracts.Messages;

/// <summary>
/// End-to-end encrypted message material. None of these fields may contain
/// plaintext or an unencrypted symmetric key.
/// </summary>
public sealed record EncryptedMessagePayload
{
    public string EncryptedKey { get; init; } = string.Empty;
    public string Iv { get; init; } = string.Empty;
    public string Ciphertext { get; init; } = string.Empty;
    public string Signature { get; init; } = string.Empty;
}
