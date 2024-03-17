cmake_minimum_required(VERSION 3.22)

add_library(Messages Message.cpp Message.h
        BeginSessionRequest.cpp
        BeginSessionRequest.h
        Response.cpp
        Response.h)
target_link_libraries(Messages PRIVATE msgpack-cxx)
target_include_directories(Messages INTERFACE .)