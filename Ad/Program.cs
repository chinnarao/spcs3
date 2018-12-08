using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
// https://stackoverflow.com/questions/46395192/is-specifying-the-listening-http-port-via-useurls-the-correct-way
namespace Ad
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, builder) =>
            {
                builder.AddFile("Logs/Ad-{Date}.txt");
            })
            .UseStartup<Startup>();
    }
}
