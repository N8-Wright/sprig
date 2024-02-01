add_library(sprig_core sprig_packet.c sprig_socket_unix.c)
target_compile_options(sprig_core PRIVATE
  $<$<CXX_COMPILER_ID:MSVC>:/W4 /WX>
  $<$<NOT:$<CXX_COMPILER_ID:MSVC>>:-Wall -Wextra -g>
)

add_executable(sprig_client sprig_client.c)
target_link_libraries(sprig_client PRIVATE sprig_core)
