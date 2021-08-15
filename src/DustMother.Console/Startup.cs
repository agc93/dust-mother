using Microsoft.Extensions.DependencyInjection;
using Spectre.Cli.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using UnSave;
using UnSave.Serialization;

namespace DustMother
{
    public static class Startup
    {
        internal static CommandApp GetApp() {
            var app = new CommandApp(new DependencyInjectionRegistrar(GetServices()));
            app.Configure(c => {
                c.PropagateExceptions();
                c.SetApplicationName("dm");
                c.AddCommand<CampaignCommand>("campaign");
                c.AddCommand<ConquestCommand>("conquest");
                c.AddCommand<StatisticsCommand>("statistics");
                c.AddCommand<GameSettingsCommand>("settings");
            });
            return app;
        }

        internal static IServiceCollection GetServices() {
            var services = new ServiceCollection();
            services.Scan(scan =>
            {
                scan.FromAssemblyOf<SaveSerializer>()
                    .AddClasses(classes => classes.AssignableTo<IUnrealPropertySerializer>()).AsImplementedInterfaces()
                    .WithSingletonLifetime();
                scan.FromAssemblyOf<SaveSerializer>()
                    .AddClasses(classes => classes.AssignableTo<IUnrealCollectionPropertySerializer>()).AsImplementedInterfaces()
                    .WithSingletonLifetime();
            });
            services.AddSingleton<PropertySerializer>();
            services.AddSingleton<SaveSerializer>();
            return services;
        }
    }
}