cmake_minimum_required(VERSION 3.22)

add_executable(Server main.cpp
        WsListener.cpp
        WsListener.h
        WSSession.cpp
        Session.h)
target_link_libraries(Server PRIVATE Boost::log Boost::system msgpack-cxx Messages)