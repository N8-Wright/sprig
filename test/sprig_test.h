#ifndef SPRIG_TEST_H_
#define SPRIG_TEST_H_
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#define ASSERT(expr) do {                                               \
        if (!(expr)) {                                                  \
            fprintf(stderr,                                             \
              "\"%s\" %s:%d\n", #expr, __FILE__,  __LINE__);            \
            exit(1);                                                    \
        }                                                               \
    } while(0);
#endif // SPRIG_TEST_H_
