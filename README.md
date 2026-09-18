# EVA Chat Contracts

The current MVP uses the existing five-field `ChatMessageEvent` shared by Gateway,
Storage and Delivery:

```text
ChatMessageEvent
  messageId: UUID
  senderId: UUID
  targetId: UUID (private-message recipient)
  ciphertext: string (serialized encrypted browser envelope, not plaintext)
  timestamp: UTC timestamp
```

Storage resolves the private `room_id` from `senderId` and `targetId` using
`room_members`, then stores `ciphertext` as `messages.content`. Storage sends
the event to `delivery_queue` only after the database write succeeds.

The current browser's `EncryptedPayloadV1` JSON contains `version`, `senderId`,
`recipientId`, `senderKeyId`, `recipientKeyId`, `iv`, and `ciphertext`.
Gateway, Storage and Delivery forward/store this opaque string; only the browsers
encrypt/decrypt it using the existing ECDH/AES-GCM implementation. There is no
separate encrypted-key or signature field in this active format. Do not populate
placeholder metadata or silently switch consumers to a different event type.

`roomId` is deliberately not part of the event: Storage creates/reuses the private
room for the two participants. `messageId` and the UTC `timestamp` are assigned by
Gateway. A publishing acknowledgement is not proof of database storage or receipt.

The alternative E2EE types below remain for source compatibility only. They are
NOT used by the current Gateway -> Storage -> Delivery flow and adopting them
would require a separately agreed protocol change:

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
