# EVA Chat Contracts

Version 2 defines the shared end-to-end encrypted private-message contract used
by Gateway, Storage and Delivery:

```text
ChatMessagePublishedEvent
  messageId: UUID
  senderId: UUID (set from the authenticated identity)
  targetId: UUID (private-message recipient)
  timestamp: Unix milliseconds supplied by the client
  payload.encryptedKey: string
  payload.iv: string
  payload.ciphertext: string
  payload.signature: string
```

`ChatMessageEvent` is retained only as an obsolete v1 compatibility type. New
code must use `EVA_ECS.Chat.Contracts.Events.ChatMessagePublishedEvent`.

The MVP supports private messages only. A message is associated with its sender
and target directly, so the live-delivery contract does not require a room ID.
