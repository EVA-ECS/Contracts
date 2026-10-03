using System.Text.Json;
using Chat.Contracts.Events;
using EVA_ECS.Chat.Contracts.Events;
using EVA_ECS.Chat.Contracts.Messages;
using EVA_ECS.Chat.Contracts.Requests;
using EVA_ECS.Chat.Contracts.Responses;
using Xunit;

namespace Contracts.Tests;

public sealed class SerializationTests
{
    private static readonly JsonSerializerOptions Web = new(JsonSerializerDefaults.Web);
    private static readonly EncryptedMessagePayload Payload = new() { EncryptedKey = "wrapped-key", Iv = "iv", Ciphertext = "encrypted-content", Signature = "signature" };
    private static T RoundTrip<T>(T value) => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, Web), Web)!;

    [Fact]
    public void PrivateMessageEventPreservesIdentityParticipantsTimestampAndCiphertext()
    {
        var value = new ChatMessageEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "encrypted-envelope", DateTime.UtcNow);
        Assert.Equal(value, RoundTrip(value));
        using var json = JsonDocument.Parse(JsonSerializer.Serialize(value, Web));
        Assert.Equal(new[] { "ciphertext", "messageId", "senderId", "targetId", "timestamp" }, json.RootElement.EnumerateObject().Select(x => x.Name).Order());
    }

    [Fact]
    public void PublishedEventAndSendRequestPreserveEncryptedMaterialWithoutSenderInRequest()
    {
        var id = Guid.NewGuid(); var sender = Guid.NewGuid(); var target = Guid.NewGuid();
        var published = new ChatMessagePublishedEvent { MessageId = id, SenderId = sender, TargetId = target, Timestamp = 1234, Payload = Payload };
        Assert.Equal(published, RoundTrip(published));
        var request = new SendMessageRequest { MessageId = id, TargetId = target, Timestamp = 1234, Payload = Payload };
        Assert.Equal(request, RoundTrip(request));
        Assert.DoesNotContain("senderId", JsonSerializer.Serialize(request, Web));
        Assert.Equal(Payload, RoundTrip(Payload));
        Assert.Equal(new EncryptedMessagePayload(), RoundTrip(new EncryptedMessagePayload()));
        Assert.Equal(new SendMessageRequest(), RoundTrip(new SendMessageRequest()));
        Assert.Equal(new ChatMessagePublishedEvent(), RoundTrip(new ChatMessagePublishedEvent()));
    }

    [Fact]
    public void RoomRequestsPreservePerParticipantWrappedKeys()
    {
        var user = Guid.NewGuid();
        var request = new CreateRoomRequest { RoomName = "Room", IsGroupChat = true, EncryptedRoomKeys = new() { [user] = "wrapped-key" } };
        var restored = RoundTrip(request);
        Assert.Equal("Room", restored.RoomName); Assert.True(restored.IsGroupChat);
        Assert.Equal("wrapped-key", restored.EncryptedRoomKeys[user]);
        var empty = RoundTrip(new CreateRoomRequest()); Assert.Null(empty.RoomName); Assert.Empty(empty.EncryptedRoomKeys); Assert.False(empty.IsGroupChat);
    }

    [Fact]
    public void DirectoryHistoryAndStatusPreserveTheirWireFields()
    {
        var user = new UserSearchResponse { UserId = Guid.NewGuid(), Username = "Alice", PublicKey = "public-key", IsOnline = true, LastSeen = DateTimeOffset.UtcNow };
        Assert.Equal(user, RoundTrip(user)); Assert.Equal(new UserSearchResponse(), RoundTrip(new UserSearchResponse()));
        var history = new MessageHistoryResponse { MessageId = Guid.NewGuid(), SenderId = user.UserId, EncryptedPayload = "ciphertext", Signature = "signature", Timestamp = DateTimeOffset.UtcNow };
        Assert.Equal(history, RoundTrip(history)); Assert.Equal(new MessageHistoryResponse(), RoundTrip(new MessageHistoryResponse()));
        var status = new UserStatusChangedEvent { UserId = user.UserId, IsOnline = false, Timestamp = DateTimeOffset.UtcNow };
        Assert.Equal(status, RoundTrip(status));
    }

    [Fact]
    public void RoomDetailsPreserveParticipantKeysAndMetadata()
    {
        var participant = new RoomParticipantDto { UserId = Guid.NewGuid(), Username = "Alice", EncryptedRoomKey = "wrapped-key" };
        Assert.Equal(participant, RoundTrip(participant)); Assert.Equal(new RoomParticipantDto(), RoundTrip(new RoomParticipantDto()));
        var room = new RoomDetailResponse { RoomId = Guid.NewGuid(), RoomName = "Room", IsGroupChat = true, CreatedAt = DateTimeOffset.UtcNow, Participants = [participant] };
        var restored = RoundTrip(room);
        Assert.Equal(room.RoomId, restored.RoomId); Assert.Equal(room.CreatedAt, restored.CreatedAt); Assert.True(restored.IsGroupChat);
        Assert.Equal(room.RoomName, restored.RoomName); Assert.Equal(participant, Assert.Single(restored.Participants));
        Assert.Empty(RoundTrip(new RoomDetailResponse()).Participants);
    }
}
