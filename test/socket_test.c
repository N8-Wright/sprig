#include "sprig_socket.h"


int main(int argc, char *argv[]) {
    sprig_socket sock = sprig_socket_open("www.google.com", 443);
}
