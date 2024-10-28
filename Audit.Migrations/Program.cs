using Audit.Infra.Data;
using Audit.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
           .AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(Configuration.ConnectionString))
           .BuildServiceProvider();


using (var context = serviceProvider.GetRequiredService<AppDbContext>())
{
    context.Database.Migrate();
    Console.WriteLine("Migrations aplicadas com sucesso!");
}