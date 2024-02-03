#include "sprig_socket.h"
#include <arpa/inet.h>
#include <netdb.h>
#include <netinet/in.h>
#include <stdio.h>
#include <string.h>
#include <sys/socket.h>
#include <sys/types.h>

static sprig_socket sprig_socket_create(const char *url, const char *port, struct addrinfo **res);

sprig_socket sprig_socket_bind(const char *url, const char *port) {
    struct addrinfo *res;
    sprig_socket sock = sprig_socket_create(url, port, &res);
    if (bind(sock.handle, (struct sockaddr *)res, sizeof(*res)) == -1) {
        perror("bind failed");
    }

    return sock;
}

sprig_socket sprig_socket_connect(const char *url, const char *port) {
    struct addrinfo *res;
    sprig_socket sock = sprig_socket_create(url, port, &res);
    if (connect(sock.handle, (struct sockaddr *)res, sizeof(*res)) == -1) {
        perror("connect failed");
    }

    return sock;
}

static sprig_socket sprig_socket_create(const char *url, const char *port, struct addrinfo **res) {
    struct addrinfo hints;
    int status;

    memset(&hints, 0, sizeof hints);
    hints.ai_family = AF_UNSPEC;// Use ipv4 or ipv6
    hints.ai_socktype = SOCK_STREAM;
    hints.ai_flags = AI_PASSIVE;// Fill in IP for me

    if ((status = getaddrinfo(url, port, &hints, res)) != 0) {
        fprintf(stderr, "getaddrinfo: %s\n", gai_strerror(status));
    }

    sprig_socket sock;
    sock.handle = socket((*res)->ai_family, (*res)->ai_socktype, (*res)->ai_protocol);
    return sock;
}
