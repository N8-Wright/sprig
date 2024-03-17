#pragma once


#include "TcpConnection.h"
#include <boost/asio/io_service.hpp>
#include <boost/asio/ip/tcp.hpp>
class TcpServer
{
public:
	TcpServer(boost::asio::io_service& io_service, int port);
private:
	void StartAccept();
	void HandleAccept(const TcpConnection::Pointer& newConnection, const boost::system::error_code& error);

	boost::asio::ip::tcp::acceptor m_acceptor;
};
