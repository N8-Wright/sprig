#pragma once
#include <msgpack.hpp>

struct Message
{
	enum class Kind
	{
		Unknown,
		BeginSessionRequest,
		Response,
	};

	Message();
	Message(Kind messageKind);

	uint16_t ProtocolVersion;
	Kind MessageKind;

	MSGPACK_DEFINE_MAP(ProtocolVersion, MessageKind);
};

MSGPACK_ADD_ENUM(Message::Kind)