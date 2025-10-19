using Ludo.Commands;
using Ludo.Models;
using Ludo.Services;
using Ludo.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ludo.ViewModels
{
    public class JoinGameViewModel : ViewModelBase
    {
        public ICommand NavigateStartScreenCommand { get; }
        GameService ObjGameService;
        public JoinGameViewModel(NavigationStore navigationStore)
        {
            //navigation
            this.NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore, () => new StartScreenViewModel(navigationStore));
            ObjGameService = new GameService();
            LoadData();
            deleteGameCommand = new RelayCommand<Game>(DeleteGame);
        }

        #region Display games
        private ObservableCollection<Game> gamesList;
        public ObservableCollection<Game> GamesList
        {
            get { return gamesList; }
            set { gamesList = value; OnPropertyChanged("GamesList"); }
        }

        //helpermethod display games
        private void LoadData()
        {
            GamesList = ObjGameService.getAll();
        }
        #endregion

        #region Delete games
        private RelayCommand<Game> deleteGameCommand;

        public RelayCommand<Game> DeleteGameCommand
        {
            get { return deleteGameCommand; }
        }

        //param game is passed with command.parameter (note we need a generic relaycommand here sicne delete taks a param)
        public void DeleteGame(Game game)
        {
            //call gameservice
            Console.WriteLine(game.Game_Name);
            LoadData(); //reload displays

        }



        #endregion




    }
}
