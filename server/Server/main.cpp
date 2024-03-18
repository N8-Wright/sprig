#include "WSListener.h"
#include <boost/asio.hpp>
#include <iostream>
#include <thread>
int main()
{
	try
	{
		auto const address = boost::asio::ip::make_address("0.0.0.0");
		const short port = 9876;
		auto const threads = static_cast<int>(std::thread::hardware_concurrency());
		boost::asio::io_service ioService{threads};

		std::make_shared<WSListener>(ioService, boost::asio::ip::tcp::endpoint{address, port})->Run();

		// Run the I/O service on the requested number of threads
		std::vector<std::thread> v;
		v.reserve(threads - 1);
		for (auto i = threads - 1; i > 0; --i)
			v.emplace_back(
					[&ioService] {
						ioService.run();
					});
		ioService.run();

		return EXIT_SUCCESS;
	}
	catch (std::exception& e)
	{
		std::cerr << e.what() << std::endl;
		return -1;
	}
	return 0;
}
