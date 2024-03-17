#pragma once
#include <boost/asio.hpp>
#include <string_view>
#include <msgpack.hpp>
#include <sstream>

class TcpClient
{
public:
	TcpClient(boost::asio::io_context& ioContext, std::string_view address, std::string_view port);
	void Send(const auto& message)
	{
		std::stringstream ss;
		msgpack::pack(ss, message);
		m_socket.write_some(boost::asio::buffer(ss.str()));
	}

private:
	boost::asio::ip::tcp::socket m_socket;
};
