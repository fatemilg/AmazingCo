using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AmazingCo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (System.Exception)
            {

                throw;
            }
         
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
