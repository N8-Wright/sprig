#include "sprig_packet.h"
#include "sprig_test.h"
void encode_ping_packet() {
    sprig_packet_ping ping = sprig_packet_ping_create(123);
    u8 bytes[SPRIG_PACKET_MAX_SIZE] = {};
    u8 bytes2[SPRIG_PACKET_MAX_SIZE] = {};
    ASSERT(sprig_packet_ping_encode(&ping, (u8*)&bytes) == SPRIG_PACKET_PING_SIZE);
    ASSERT(sprig_packet_encode((sprig_packet*)&ping, (u8*)&bytes2) == SPRIG_PACKET_PING_SIZE);
    ASSERT(0 == memcmp(bytes, bytes2, SPRIG_PACKET_PING_SIZE));
}

void encode_response_packet() {
    sprig_packet_response res = sprig_packet_response_create(SPRIG_RESPONSE_SERVER_ERROR);
    u8 bytes[SPRIG_PACKET_MAX_SIZE] = {};
    u8 bytes2[SPRIG_PACKET_MAX_SIZE] = {};
    ASSERT(sprig_packet_response_encode(&res, (u8*)&bytes) == SPRIG_PACKET_RESPONSE_SIZE);
    ASSERT(sprig_packet_encode((sprig_packet*)&res, (u8*)&bytes2) == SPRIG_PACKET_RESPONSE_SIZE);
    ASSERT(0 == memcmp(bytes, bytes2, SPRIG_PACKET_PING_SIZE));
}

int main(int argc, char* argv[]) {
    encode_ping_packet();
    encode_response_packet();
    return 0;
}
