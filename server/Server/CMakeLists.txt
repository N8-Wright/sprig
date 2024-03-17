cmake_minimum_required(VERSION 3.22)

add_executable(Server main.cpp
        TcpServer.cpp
        TcpServer.h
        TcpConnection.cpp
        TcpConnection.h)
target_link_libraries(Server PRIVATE msgpack-cxx Messages)