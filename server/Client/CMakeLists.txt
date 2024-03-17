cmake_minimum_required(VERSION 3.22)

add_executable(Client main.cpp
        TcpClient.cpp
        TcpClient.h)
target_link_libraries(Client PRIVATE msgpack-cxx Boost::log Boost::system Messages)