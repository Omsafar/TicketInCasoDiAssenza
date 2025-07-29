using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketingApp.Data;
using TicketingApp.Graph;
using TicketingApp.Services;

internal class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();

        services.AddDbContext<TicketContext>(options =>
            options.UseSqlServer("Server=192.168.1.24\\sgam;Database=PARATORI;User Id=sapara;Password=S@p4ra;Encrypt=True;TrustServerCertificate=True;"));

        services.AddScoped<TicketRepository>();
        services.AddSingleton(s => new GraphMailReader("support.ticket@paratorispa.it"));
        services.AddSingleton<GraphMailSender>();
        services.AddSingleton<TicketManager>();

        var provider = services.BuildServiceProvider();

        await GraphAuthProvider.InitializeAsync();

        var manager = provider.GetRequiredService<TicketManager>();
        manager.CurrentUserEmail = GraphAuthProvider.CurrentUserEmail ?? string.Empty;
        manager.IsAdmin = true;
        manager.CanSync = true;

        await manager.SyncAsync(CancellationToken.None);
    }
}