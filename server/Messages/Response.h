#pragma once
#include "Message.h"
#include <msgpack.hpp>
#include <string>

struct Response : public Message
{
	enum class Kind
	{
		Unknown,
		Ok,
		InternalServerError,
		InvalidRequest,
	};

	Response();
	Response(Kind code);
	Response(Kind code, std::string info);

	Kind Code;
	std::string Info;
	MSGPACK_DEFINE_MAP(MSGPACK_BASE_MAP(Message), Code, Info)
};

MSGPACK_ADD_ENUM(Response::Kind);
