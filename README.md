# EVA Chat Contracts

The current MVP uses the simple plaintext `ChatMessageEvent` shared by Gateway,
Storage and Delivery:

```text
ChatMessageEvent
  messageId: UUID
  senderId: UUID
  targetId: UUID (private-message recipient)
  ciphertext: string (plain text in this MVP)
  timestamp: UTC timestamp
```

Storage resolves the private `room_id` from `senderId` and `targetId` using
`room_members`, then stores `ciphertext` as `messages.content`. Storage sends
the event to `delivery_queue` only after the database write succeeds.

The later E2EE contract remains available for a future protocol version:

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

The MVP supports private messages only. Group messages are outside the current
scope. A message is associated with its sender and target directly; Storage is
responsible for resolving the corresponding private room.
