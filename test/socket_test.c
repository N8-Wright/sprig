#include "sprig_socket.h"
#include <stdlib.h>
int main(int argc, char *argv[]) {
    sprig_socket sock = sprig_socket_bind("www.google.com", "443");
    sprig_socket local_sock = sprig_socket_bind(NULL, "10");
}
