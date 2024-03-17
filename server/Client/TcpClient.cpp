#include "TcpClient.h"

using namespace boost::asio::ip;
using namespace boost::asio;
TcpClient::TcpClient(io_context &ioContext, std::string_view address, std::string_view port)
	: m_socket(ioContext)
{
	tcp::resolver resolver(ioContext);
	const auto endpoints = resolver.resolve("localhost", "9876");
	connect(m_socket, endpoints);
}
