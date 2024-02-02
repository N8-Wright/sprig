#include "sprig_socket.h"

int main(int argc, char* argv[]) {
    if (argc != 3) {
        return 0;
    }

    const char* address = argv[1];
    const char* port = argv[2];
    sprig_socket sock = sprig_socket_bind(address, port);
}