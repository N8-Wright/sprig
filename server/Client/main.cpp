#include "BeginSessionRequest.h"
#include "TcpClient.h"
#include <boost/asio.hpp>
#include <boost/log/trivial.hpp>
#include <iostream>

using namespace boost::asio::ip;
int main()
{
	try
	{
		boost::asio::io_context ioContext;
		TcpClient client(ioContext, "localhost", "9876");
		BeginSessionRequest message("Hello, world!");
		while (1)
		{
			client.Send(message);
		}
	}
	catch (std::exception& e)
	{
		BOOST_LOG_TRIVIAL(fatal) << e.what();
		return -1;
	}
	return 0;
}
