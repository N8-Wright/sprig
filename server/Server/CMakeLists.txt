cmake_minimum_required(VERSION 3.22)

add_executable(Server main.cpp
        WSListener.cpp
        WSListener.h
        WSSession.cpp
        WSSession.h)
target_link_libraries(Server PRIVATE Boost::log Boost::system msgpack-cxx Messages)