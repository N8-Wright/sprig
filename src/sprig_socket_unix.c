#include "sprig_socket.h"
#include <arpa/inet.h>
#include <netdb.h>
#include <netinet/in.h>
#include <stdio.h>
#include <string.h>
#include <sys/socket.h>
#include <sys/types.h>

sprig_socket sprig_socket_bind(const char *url, const char *port) {
    struct addrinfo hints, *res, *p;
    int status;

    memset(&hints, 0, sizeof hints);
    hints.ai_family = AF_UNSPEC;// Use ipv4 or ipv6
    hints.ai_socktype = SOCK_STREAM;
    hints.ai_flags = AI_PASSIVE;// Fill in IP for me

    if ((status = getaddrinfo(url, port, &hints, &res)) != 0) {
        fprintf(stderr, "getaddrinfo: %s\n", gai_strerror(status));
    }

    sprig_socket sock;
    sock.handle = socket(res->ai_family, res->ai_socktype, res->ai_protocol);
    if (sock.handle == -1) {
        return sock;
    }

    if (bind(sock.handle, (struct sockaddr *)res, sizeof(*res)) == -1) {
        fprintf(stderr, "bind failed!");
    }

    return sock;
}
