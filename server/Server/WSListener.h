#pragma once

#include <boost/asio.hpp>
#include <boost/asio/coroutine.hpp>
#include <boost/beast.hpp>
#include <memory>

class WSListener : public boost::asio::coroutine, public std::enable_shared_from_this<WSListener>
{
	boost::asio::io_context& m_ioContext;
	boost::asio::ip::tcp::acceptor m_acceptor;
	boost::asio::ip::tcp::socket m_socket;
	void Loop(boost::beast::error_code ec = {});

public:
	WSListener(boost::asio::io_context& ioContext, boost::asio::ip::tcp::endpoint endpoint);
	void Run();
};
