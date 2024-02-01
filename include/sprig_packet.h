#ifndef SPRIG_PACKET_H_
#define SPRIG_PACKET_H_
#include "sprig_types.h"

typedef enum {
    SPRIG_PACKET_INVALID,
    SPRIG_PACKET_PING,
    SPRIG_PACKET_RESPONSE,
    SPRIG_PACKET_CHAT,
} sprig_packet_type;

typedef enum {
    SPRIG_RESPONSE_INVALID,
    SPRIG_RESPONSE_SERVER_ERROR,
    SPRIG_RESPONSE_OK,
} sprig_response_code;

#define SPRIG_PACKET_MAX_SIZE 256
#define SPRIG_PACKET_SIZE (sizeof(u16))
#define SPRIG_PACKET_PING_SIZE (SPRIG_PACKET_SIZE + sizeof(u32))
#define SPRIG_PACKET_RESPONSE_SIZE (SPRIG_PACKET_SIZE + sizeof(u32))

typedef struct {
    u16 type;
} sprig_packet;

typedef struct {
    sprig_packet base;
    u32 response_code;
} sprig_packet_response;

typedef struct {
    sprig_packet base;
    u32 sender_id;
} sprig_packet_ping;

typedef struct {
    sprig_packet base;

    u32 message_id;

    u32 sender_id;
    u32 receiver_id;

    u32 total_packets;
    u32 current_packet;

} sprig_packet_chat;

sprig_packet_ping sprig_packet_ping_create(u32 sender_id);
sprig_packet_response sprig_packet_response_create(sprig_response_code response);

u32 sprig_packet_encode(sprig_packet* packet, u8* buffer);
u32 sprig_packet_ping_encode(sprig_packet_ping* packet, u8* buffer);
u32 sprig_packet_response_encode(const sprig_packet_response* packet, u8* buffer);
#endif // SPRIG_PACKET_H_
