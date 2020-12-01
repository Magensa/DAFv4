using DafV4.ServiceFactory;
using DafV4.UIFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DafV4.DemoApp
{
    class Program
    {
        static void Main()
        {
            _ = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<IDafV4UIFactory, Mppgv4UIfactory>();
            services.AddSingleton<IProcessCardSwipeClient, ProcessCardSwipeClient>();
            services.AddSingleton<IProcessDataClient, ProcessDataClient>();
            services.AddSingleton<IProcessTokenClient, ProcessTokenClient>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var uiFactory = serviceProvider.GetService<IDafV4UIFactory>();

            while (true)
            {
                try
                {
                    Console.WriteLine("Please Select an option or service operation");
                    Console.WriteLine("Enter Option number");
                    Console.WriteLine("1:ProcessCardSwipe");
                    Console.WriteLine("2:ProcessData");
                    Console.WriteLine("3:ProcessToken");

                    var keyInfo = Console.ReadKey();
                    Console.WriteLine();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1:
                            uiFactory.ShowUI(DafV4UI.PROCESSCARDSWIPE);
                            break;
                        case ConsoleKey.D2:
                            uiFactory.ShowUI(DafV4UI.PROCESSDATA);
                            break;
                        case ConsoleKey.D3:
                            uiFactory.ShowUI(DafV4UI.PROCESSTOKEN);
                            break;
                    }
                    bool decision = Confirm("Would you like to Continue with other Request");
                    if (decision)
                        continue;
                    else
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{ title } [y/n] ");
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return (response == ConsoleKey.Y);
        }
    }
}
