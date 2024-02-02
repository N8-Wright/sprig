#ifndef SPRIG_SOCKET_H_
#define SPRIG_SOCKET_H_
#include "sprig_packet.h"
typedef struct sprig_socket {
#ifdef _WIN32
    HANDLE handle;
#else
    int handle;
#endif
} sprig_socket;

sprig_socket sprig_socket_bind(const char *url, const char *port);
sprig_socket sprig_socket_connect(const char *url, const char *port);

void sprig_send(sprig_socket *, sprig_packet *packet);
sprig_packet *sprig_recv(sprig_socket *);
#endif// SPRIG_SOCKET_H_
