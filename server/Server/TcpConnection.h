#pragma once

#include <boost/asio/io_service.hpp>
#include <boost/asio/ip/tcp.hpp>
#include <boost/asio/streambuf.hpp>
#include <boost/enable_shared_from_this.hpp>
#include <msgpack/unpack.hpp>
class TcpConnection : public boost::enable_shared_from_this<TcpConnection>
{
public:
	typedef boost::shared_ptr<TcpConnection> Pointer;

	static Pointer Create(const boost::asio::any_io_executor& executor)
	{
		return Pointer(new TcpConnection(executor));
	}

	boost::asio::ip::tcp::socket& Socket();
	void Start();
private:
	explicit TcpConnection(const boost::asio::any_io_executor& executor);
	void HandleWrite(const boost::system::error_code& error, size_t bytesTransferred);
	void HandleRead(const boost::system::error_code& error);

	boost::asio::ip::tcp::socket m_socket;
	boost::asio::streambuf m_inputBuffer;
	class msgpack::unpacker m_unpacker;
};
