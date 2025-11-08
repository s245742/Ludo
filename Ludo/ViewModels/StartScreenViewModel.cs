using Ludo.Commands;
using Ludo.Stores;
using Ludo.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace Ludo.ViewModels
{
    public class StartScreenViewModel : ViewModelBase
    {

        public ICommand NavigateCreateGameCommand {  get; }
        public ICommand NavigateJoinGameCommand { get; }

        public StartScreenViewModel(NavigationStore navigationStore)
        {
            
            this.NavigateCreateGameCommand = new NavigateCommand<CreateGameViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<CreateGameViewModel>()); 
            this.NavigateJoinGameCommand = new NavigateCommand<JoinGameViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<JoinGameViewModel>());
        }

    }
}
