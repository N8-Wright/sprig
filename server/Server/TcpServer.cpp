#include "TcpServer.h"
#include "TcpConnection.h"
#include <boost/asio/placeholders.hpp>

using namespace boost::asio::ip;

TcpServer::TcpServer(boost::asio::io_service& io_service, int port)
	: m_acceptor(io_service, tcp::endpoint(tcp::v4(), port))
{
	StartAccept();
}

void TcpServer::StartAccept()
{
	TcpConnection::Pointer newConnection = TcpConnection::Create(m_acceptor.get_executor());
	m_acceptor.async_accept(newConnection->Socket(),
							std::bind(&TcpServer::HandleAccept, this, newConnection, boost::asio::placeholders::error));
}

void TcpServer::HandleAccept(const TcpConnection::Pointer& newConnection, const boost::system::error_code& error)
{
	if (!error)
	{
		newConnection->Start();
		StartAccept();
	}
}
