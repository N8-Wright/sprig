cmake_minimum_required(VERSION 3.27)
project(Y)

set(Boost_USE_STATIC_LIBS ON)
find_package(Boost 1.84 COMPONENTS log system REQUIRED)
find_package(msgpack-cxx REQUIRED)
set(CMAKE_CXX_STANDARD 20)

add_subdirectory(Messages)
add_subdirectory(Server)
add_subdirectory(Client)
