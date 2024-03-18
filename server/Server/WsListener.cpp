//
// Created by Nathaniel Wright on 3/18/24.
//

#include "WsListener.h"
#include "Session.h"
#include <boost/asio/coroutine.hpp>
#include <boost/asio/dispatch.hpp>
#include <boost/asio/strand.hpp>
#include <boost/beast.hpp>
#include <boost/beast/core.hpp>
#include <boost/beast/websocket.hpp>
#include <boost/log/trivial.hpp>

using namespace boost::asio;
using namespace boost::beast;

WSListener::WSListener(boost::asio::io_context& ioContext, boost::asio::ip::tcp::endpoint endpoint)
	: m_ioContext(ioContext), m_acceptor(make_strand(ioContext)), m_socket(make_strand(ioContext))
{
	boost::beast::error_code ec;

	// Open the acceptor
	m_acceptor.open(endpoint.protocol(), ec);
	if (ec)
	{
		BOOST_LOG_TRIVIAL(fatal) << ec.message();
		return;
	}

	// Allow address reuse
	m_acceptor.set_option(socket_base::reuse_address(true), ec);
	if (ec)
	{
		BOOST_LOG_TRIVIAL(fatal) << ec.message();
		return;
	}

	// Bind to the server address
	m_acceptor.bind(endpoint, ec);
	if (ec)
	{
		BOOST_LOG_TRIVIAL(fatal) << ec.message();
		return;
	}

	// Start listening for connections
	m_acceptor.listen(socket_base::max_listen_connections, ec);
	if (ec)
	{
		BOOST_LOG_TRIVIAL(fatal) << ec.message();
		return;
	}
}

void WSListener::Run()
{
	Loop();
}

#include <boost/asio/yield.hpp>
void WSListener::Loop(boost::beast::error_code ec)
{
	reenter(*this)
	{
		for (;;)
		{
			yield m_acceptor.async_accept(m_socket,
										  std::bind(&WSListener::Loop,
													shared_from_this(),
													std::placeholders::_1));
			if (ec)
			{
				BOOST_LOG_TRIVIAL(error) << ec.message();
				return;
			}
			else
			{
				std::make_shared<WSSession>(std::move(m_socket))->Run();
			}

			// Make sure each session gets its own strand
			m_socket = boost::asio::ip::tcp::socket(boost::asio::make_strand(m_ioContext));
		}
	}
}
#include <boost/asio/unyield.hpp>