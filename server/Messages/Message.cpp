#include "Message.h"

Message::Message()
	: ProtocolVersion(0), MessageKind(Kind::Unknown)
{
}

Message::Message(Message::Kind messageKind)
	: ProtocolVersion(0), MessageKind(messageKind)
{
}