add_library(sprig_core sprig_packet.c)
add_executable(sprig_client sprig_client.c)
target_link_libraries(sprig_client PRIVATE sprig_core)
