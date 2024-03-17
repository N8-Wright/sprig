#include "Response.h"

Response::Response()
	: Code(Kind::Unknown), Message(Message::Kind::Response)
{
}

Response::Response(Response::Kind code)
	: Code(code), Message(Message::Kind::Response)
{
}

Response::Response(Response::Kind code, std::string info)
	: Code(code), Info(std::move(info)), Message(Message::Kind::Response)
{
}
