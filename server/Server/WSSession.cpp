#include "Session.h"
#include <boost/log/trivial.hpp>
#include <msgpack.hpp>

WSSession::WSSession(boost::asio::ip::tcp::socket socket)
	: m_ws(std::move(socket))
{
}

void WSSession::Run()
{
	// We need to be executing within a strand to perform async operations
	// on the I/O objects in this session. Although not strictly necessary
	// for single-threaded contexts, this example code is written to be
	// thread-safe by default.
	boost::asio::dispatch(m_ws.get_executor(),
						  boost::beast::bind_front_handler(&WSSession::Loop,
														   shared_from_this(),
														   boost::beast::error_code{},
														   0));
}

#include <boost/asio/yield.hpp>
void WSSession::Loop(boost::beast::error_code ec, std::size_t bytesTransferred)
{
	boost::ignore_unused(bytesTransferred);
	reenter(*this)
	{
		// Set suggested timeout settings
		m_ws.set_option(boost::beast::websocket::stream_base::timeout::suggested(boost::beast::role_type::server));

		// Set a decorator to change the server of the handshake
		m_ws.set_option(
				boost::beast::websocket::stream_base::decorator([](boost::beast::websocket::response_type& res) {
					res.set(boost::beast::http::field::server,
							std::string(BOOST_BEAST_VERSION_STRING) + " websocket-server-stackless");
				}));

		// Accept the websocket handshake
		yield m_ws.async_accept(std::bind(&WSSession::Loop, shared_from_this(), std::placeholders::_1, 0));
		if (ec)
		{
			BOOST_LOG_TRIVIAL(error) << ec.message();
			return;
		}

		for (;;)
		{
			yield m_ws.async_read(m_buffer,
								  std::bind(&WSSession::Loop,
											shared_from_this(),
											std::placeholders::_1,
											std::placeholders::_2));
			if (ec == boost::beast::websocket::error::closed)
			{
				// This indicates that the session was closed
				return;
			}
			else if (ec)
			{
				BOOST_LOG_TRIVIAL(error) << ec.message();
				return;
			}

			// Echo the message
			m_ws.text(m_ws.got_text());
			m_obj = msgpack::unpack(static_cast<char*>(m_buffer.data().data()), m_buffer.data().size());
			BOOST_LOG_TRIVIAL(info) << m_obj.get();

			yield m_ws.async_write(m_buffer.data(),
								   std::bind(&WSSession::Loop,
											 shared_from_this(),
											 std::placeholders::_1,
											 std::placeholders::_2));
			if (ec)
			{
				BOOST_LOG_TRIVIAL(error) << ec.message();
				return;
			}

			// Clear the buffer
			m_buffer.consume(m_buffer.size());
		}
	}
}
#include <boost/asio/unyield.hpp>