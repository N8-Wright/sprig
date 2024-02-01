include(CTest)

add_executable(packet_encoding packet_encoding.c)
target_link_libraries(packet_encoding PRIVATE sprig_core)
add_test(NAME packet_encoding COMMAND packet_encoding)

target_compile_options(packet_encoding PRIVATE
  $<$<CXX_COMPILER_ID:MSVC>:/W4 /WX>
  $<$<NOT:$<CXX_COMPILER_ID:MSVC>>:-Wall -Wextra -g>
)

add_executable(socket_test socket_test.c)
target_link_libraries(socket_test PRIVATE sprig_core)
add_test(NAME socket_test COMMAND socket_test)

target_compile_options(socket_test PRIVATE
  $<$<CXX_COMPILER_ID:MSVC>:/W4 /WX>
  $<$<NOT:$<CXX_COMPILER_ID:MSVC>>:-Wall -Wextra -g>
)


