#include "cbstd.h"
#include <iostream>
#include <fstream>
#include <sstream>

void cb_print(const cb_string& str)
{
	std::cout << str << '\n';
}

void cb_exit(const i32& exit_code)
{
	std::exit(exit_code);
}

void cb_breakpoint(const cb_string& msg)
{
	if (!msg.empty())
	{
		cb_print("Breakpoint: " + msg);
	}
	else
	{
		cb_print("Breakpoint hit.");
	}

	cb_print("Execution paused. Press Enter to continue...");
	std::cin.get();
}

void cb_breakpoint(const bool& condition, const cb_string& msg)
{
	if (condition)
	{
		cb_breakpoint(msg);
	}
}

cb_string cb_read_file(const cb_string& path)
{
	std::ifstream file(path, std::ios::binary);
	if (!file)
	{
		throw std::runtime_error("Failed to open file: " + path);
	}

	std::ostringstream buffer;
	buffer << file.rdbuf();
	return buffer.str();
}

cb_string cb_tostr(const i8& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const i16& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const i32& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const i64& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const u8& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const u16& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const u32& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const u64& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const f32& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const f64& value)
{
	return std::to_string(value);
}

cb_string cb_tostr(const f128& value)
{
	return std::to_string(value);
}

u32 cb_strlen(const cb_string& str)
{
	return static_cast<u32>(str.length());
}

void cb_replace(cb_string& str, const cb_string& replacement)
{
	str = replacement;
}


