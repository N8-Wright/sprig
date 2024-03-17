#pragma once
#include "Message.h"
#include <msgpack.hpp>
struct BeginSessionRequest : public Message
{
	std::string SessionId;
	BeginSessionRequest();
	BeginSessionRequest(std::string sessionId);
	
	MSGPACK_DEFINE_MAP(MSGPACK_BASE_MAP(Message), SessionId);
};
