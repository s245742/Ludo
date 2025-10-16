using Ludo.Commands;
using Ludo.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ludo.ViewModels
{
    public class StartScreenViewModel : ViewModelBase
    {

        public ICommand NavigateCreateGameCommand {  get; }
        public ICommand NavigateJoinGameCommand { get; }

        public StartScreenViewModel(NavigationStore navigationStore)
        {
            //nav to game view model
            this.NavigateCreateGameCommand = new NavigateCommand<CreateGameViewModel>(navigationStore, () => new CreateGameViewModel(navigationStore)); 
            this.NavigateJoinGameCommand = new NavigateCommand<JoinGameViewModel>(navigationStore, () => new JoinGameViewModel(navigationStore));
        }

    }
}
