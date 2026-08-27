# EVA Chat Contracts

Version 2 defines the shared end-to-end encrypted private-message contract used
by Gateway, Storage and Delivery:

```text
ChatMessagePublishedEvent
  messageId: UUID
  roomId: UUID
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

The current contract intentionally does not invent group-recipient envelopes.
Live group delivery requires a separately agreed representation of the room
recipients and their per-recipient encrypted keys.
