#ifndef SPRIG_TYPES_H_
#define SPRIG_TYPES_H_
#define offsetof(st, m) \
    __builtin_offsetof(st, m)

typedef unsigned char u8;
typedef signed char s8;
typedef unsigned short u16;
typedef signed short s16;
typedef unsigned int u32;
typedef signed int s32;
#endif /* SPRIG_TYPES_H_ */
