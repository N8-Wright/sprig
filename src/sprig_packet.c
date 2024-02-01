#include "sprig_packet.h"
#include <stdlib.h>
#include <string.h>
#include <arpa/inet.h>

sprig_packet_ping sprig_packet_ping_create(u32 sender_id) {
    sprig_packet_ping ping;
    ping.base.type = SPRIG_PACKET_PING;
    ping.sender_id = sender_id;
    return ping;
}

sprig_packet_response sprig_packet_response_create(sprig_response_code response) {
    sprig_packet_response res;
    res.base.type = SPRIG_PACKET_RESPONSE;
    res.response_code = response;
    return res;
}

u32 sprig_packet_encode(sprig_packet* packet, u8* buffer) {
    switch (packet->type) {
    case SPRIG_PACKET_PING:
        return sprig_packet_ping_encode((sprig_packet_ping*)packet, buffer);
    case SPRIG_PACKET_RESPONSE:
        return sprig_packet_response_encode((sprig_packet_response*)packet, buffer);
    default:
        return 0;
    }
}

u32 sprig_packet_ping_encode(sprig_packet_ping* packet, u8* buffer) {
    u16 type = htons(packet->base.type);
    u32 sender_id = htonl(packet->sender_id);

    memcpy(&buffer[offsetof(sprig_packet_ping, base.type)],
      &type,
      sizeof(type));
    memcpy(&buffer[offsetof(sprig_packet_ping, sender_id)],
      &sender_id,
      sizeof(sender_id));
    return SPRIG_PACKET_PING_SIZE;
}

u32 sprig_packet_response_encode(const sprig_packet_response* packet, u8* buffer) {
    u16 type = htons(packet->base.type);
    u32 response_code = htonl(packet->response_code);

    memcpy(&buffer[offsetof(sprig_packet_response, base.type)],
      &type,
      sizeof(type));
    memcpy(&buffer[offsetof(sprig_packet_response, response_code)],
      &response_code,
      sizeof(response_code));
    return SPRIG_PACKET_RESPONSE_SIZE;
}
