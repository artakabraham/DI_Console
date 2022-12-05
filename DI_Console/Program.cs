
using DI_Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
      .ConfigureServices((_, services) =>
      services.AddTransient<ITransientOperation, DefaultOperation>()
              .AddScoped<IScopedOperation, DefaultOperation>()
              .AddSingleton<ISingletonOperation, DefaultOperation>()
              .AddTransient<OperationLogger>())
      .Build();

ExemplifyScoping(host.Services, "Scope 1");
ExemplifyScoping(host.Services, "Scope 2");

await host.RunAsync();


static void ExemplifyScoping(IServiceProvider service, string scope)
{
    using IServiceScope serviceScope = service.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    OperationLogger logger = provider.GetRequiredService<OperationLogger>();
    logger.LogOperations($"{scope}-Call 1 .GetRequiredService<OperationLogger>()");

    Console.WriteLine("...");

    logger = provider.GetRequiredService<OperationLogger>();
    logger.LogOperations($"{scope}-Call 2 .GetRequiredService<OperationLogger>()");

    Console.WriteLine();
}