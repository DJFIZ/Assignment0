using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Assignment0.Areas.Identity.IdentityHostingStartup))]
namespace Assignment0.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}