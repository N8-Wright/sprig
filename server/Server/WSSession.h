#pragma once
#include <boost/asio/coroutine.hpp>
#include <boost/beast/core.hpp>
#include <boost/beast/websocket.hpp>
#include <memory>
#include <msgpack.hpp>

class WSSession : public boost::asio::coroutine, public std::enable_shared_from_this<WSSession>
{
	boost::beast::websocket::stream<boost::beast::tcp_stream> m_ws;
	boost::beast::flat_buffer m_buffer;
	msgpack::object_handle m_obj;

	void Loop(boost::beast::error_code ec, std::size_t bytesTransferred);

public:
	explicit WSSession(boost::asio::ip::tcp::socket socket);
	void Run();
};
