#ifndef UNITY_NATIVE_API_WRAPPER_H_
#define UNITY_NATIVE_API_WRAPPER_H_

typedef void* (DotNetCoAllocFunc)(size_t size);
typedef void (DotNetCoFreeFunc)(void* ptr);

typedef struct UnityApi
{
    // Allocate memory that can be freed by .net marshaling
    DotNetCoAllocFunc* CoAlloc;
    DotNetCoFreeFunc* CoFree;
} UnityApi;

// TODO:
//   Ability to call err viewer from C++ directly

#endif
