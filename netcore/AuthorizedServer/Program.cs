using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AuthorizedServer
{
    /// <summary></summary>
    public class Program
    {
        /// <summary></summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseUrls("http://0.0.0.0:5001")
                .Build();
    }
}
