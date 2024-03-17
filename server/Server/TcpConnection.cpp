//
// Created by Nathaniel Wright on 3/16/24.
//

#include "TcpConnection.h"
#include "Response.h"
#include <boost/asio/placeholders.hpp>
#include <boost/asio/read.hpp>
#include <iostream>
#include <msgpack.hpp>

using namespace boost::asio;
using namespace boost::asio::ip;
using namespace boost::system;
static void Dispatch(tcp::socket& socket, const msgpack::object& deserialized)
{
	std::cout << "request: " << deserialized << std::endl;

	std::stringstream ss;
	Response res(Response::Kind::Ok, "Hey this is all good!");
	msgpack::pack(ss, res);
	socket.write_some(boost::asio::buffer(ss.str()));
}

TcpConnection::TcpConnection(const boost::asio::any_io_executor& executor)
	: m_socket(executor)
{
	m_unpacker.reserve_buffer();
}

tcp::socket& TcpConnection::Socket()
{
	return m_socket;
}

void TcpConnection::Start()
{
	boost::asio::async_read(m_socket, m_inputBuffer,
							boost::asio::transfer_at_least(1),
							std::bind(&TcpConnection::HandleRead, shared_from_this(),
									  placeholders::error));
}

void TcpConnection::HandleWrite(const error_code& error, size_t bytesTransferred)
{
	(void)error;
	(void)bytesTransferred;
}

void TcpConnection::HandleRead(const error_code& error)
{
	if (!error)
	{
		// https://stackoverflow.com/a/3203502/4288232
		std::istream is(&m_inputBuffer);
		std::string line(std::istreambuf_iterator<char>(is), {});

		//Feed data into msgpack unpacker
		if (line.size() > m_unpacker.buffer_capacity())
		{
			m_unpacker.reserve_buffer(line.size());
		}

		memcpy(m_unpacker.buffer(), line.data(), line.size());
		m_unpacker.buffer_consumed(line.size());

		//Check if any objects are complete
		msgpack::object_handle result;
		while (m_unpacker.next(result))
		{
			msgpack::object deserialized(result.get());
			Dispatch(m_socket, deserialized);
		}

		//Prepare for next read
		boost::asio::async_read(m_socket, m_inputBuffer,
								boost::asio::transfer_at_least(1),
								std::bind(&TcpConnection::HandleRead, shared_from_this(),
										  placeholders::error));
	}
}