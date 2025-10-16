using System.Configuration;
using System.Data;
using System.Windows;
using Ludo.Stores;
using Ludo.ViewModels;
using Ludo.Views;

namespace Ludo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
                NavigationStore navigationStore = new NavigationStore();
                navigationStore.CurrentViewModel = new StartScreenViewModel(navigationStore);
            
                MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore) 
            };
            MainWindow.Show();


            base.OnStartup(e);
        }

    }

}
