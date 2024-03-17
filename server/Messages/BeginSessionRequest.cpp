#include "BeginSessionRequest.h"

BeginSessionRequest::BeginSessionRequest()
	: Message(Message::Kind::BeginSessionRequest)
{
}

BeginSessionRequest::BeginSessionRequest(std::string sessionId)
	: SessionId(std::move(sessionId)), Message(Message::Kind::BeginSessionRequest)
{
}
