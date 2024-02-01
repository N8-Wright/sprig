cmake_minimum_required(VERSION 3.0)
project(Sprig LANGUAGES C)

set(CMAKE_C_STANDARD 11)

include_directories(include)
add_subdirectory(src)
add_subdirectory(test)
