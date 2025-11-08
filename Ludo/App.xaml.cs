using Ludo.Services;
using Ludo.Stores;
using Ludo.ViewModels.InGameViewModels;
using Ludo.ViewModels.PreGameViewModels;
using Ludo.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Ludo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            //setup dependency injection for services
            var services = new ServiceCollection(); //DI container

            //DI register navigation og currentPlayers store
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<CurrPlayersStore>();

            //DI register services
            services.AddSingleton<GameService>();
            services.AddSingleton<PlayerService>();
            services.AddSingleton<GamePieceService>();

            //DI register viewmodels when navigation switches viewmodels (transient so it makes new instance each time called)
            services.AddTransient<StartScreenViewModel>();
            services.AddTransient<CreateGameViewModel>();
            services.AddTransient<JoinGameViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>();
            services.AddTransient<GameViewModel>();

            //finallize DI 
            ServiceProvider = services.BuildServiceProvider();

            //setup navigation
            var navigationStore = ServiceProvider.GetRequiredService<NavigationStore>();
            navigationStore.CurrentViewModel = ServiceProvider.GetRequiredService<StartScreenViewModel>();

            //setup main window
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

    }

}
