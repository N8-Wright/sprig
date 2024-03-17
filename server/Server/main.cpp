#include "TcpServer.h"
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <iostream>

int main()
{
	try
	{
		boost::asio::io_service ioService;
		TcpServer server(ioService, 9876);
		ioService.run();
	}
	catch (std::exception& e)
	{
		std::cerr << e.what() << std::endl;
		return -1;
	}
	return 0;
}
