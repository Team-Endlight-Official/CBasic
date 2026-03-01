#pragma once

#ifndef CBSTD
#define CBSTD

#include <cstdint>
#include <string>

// DEVELPER CHEATSHEET
// Ref: foo(T& refVar); // pass by reference
// In: foo(const T& refVar); // pass by const reference | (prevents modification of the argument)
// Out foo(T& refVar); // pass by reference | (allows modification of the argument)

// CBasic Runtime Standard Library
// Typedefs

typedef uint8_t u8;
typedef uint16_t u16;
typedef uint32_t u32;
typedef uint64_t u64;

typedef int8_t i8;
typedef int16_t i16;
typedef int32_t i32;
typedef int64_t i64;

typedef float f32;
typedef double f64;
typedef long double f128;

typedef std::string cb_string;

// CBasic Runtime Standard Library
// Standard Functions

// Raw Functions

void cb_print(const cb_string& str);
void cb_exit(const i32& exit_code);
void cb_breakpoint(const cb_string& msg);
void cb_breakpoint(const bool& condition, const cb_string& msg);

// String operations/functions

cb_string cb_read_file(const cb_string& path);

cb_string cb_tostr(const i8& value);
cb_string cb_tostr(const i16& value);
cb_string cb_tostr(const i32& value);
cb_string cb_tostr(const i64& value);

cb_string cb_tostr(const u8& value);
cb_string cb_tostr(const u16& value);
cb_string cb_tostr(const u32& value);
cb_string cb_tostr(const u64& value);

cb_string cb_tostr(const f32& value);
cb_string cb_tostr(const f64& value);
cb_string cb_tostr(const f128& value);

u32 cb_strlen(const cb_string& str);
void cb_replace(cb_string& str, const cb_string& replacement);


#endif // !CBSTD
