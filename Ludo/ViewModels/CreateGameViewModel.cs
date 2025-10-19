using Ludo.Commands;
using Ludo.Models;
using Ludo.Services;
using Ludo.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Ludo.ViewModels
{
    public class CreateGameViewModel : ViewModelBase
    {
        
        public ICommand NavigateStartScreenCommand { get; }

        GameService ObjGameService;
        public CreateGameViewModel(NavigationStore navigationStore)
        {
            //navigation
            this.NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore, () => new StartScreenViewModel(navigationStore));

            ObjGameService = new GameService(); //can now call method

            CurrGame = new Game();
            RedPlayer = new Player("Red");
            Yellowplayer = new Player("Yellow");
            GreenPlayer = new Player("Green");
            BluePlayer = new Player("Blue");

            saveCommand = new RelayCommand(Save);
        }

        #region CreatePlayers
        private Player redPlayer;

        public Player RedPlayer
        {
            get { return redPlayer; }
            set { redPlayer = value; OnPropertyChanged(nameof(RedPlayer)); }
        }

        private Player greenPlayer;

        public Player GreenPlayer
        {
            get { return greenPlayer; }
            set { greenPlayer = value; OnPropertyChanged(nameof(GreenPlayer)); }
        }

        private Player yellowPlayer;

        public Player Yellowplayer
        {
            get { return yellowPlayer; }
            set { yellowPlayer = value; OnPropertyChanged(nameof(Yellowplayer)); }
        }

        private Player bluePlayer;

        public Player BluePlayer
        {
            get { return bluePlayer; }
            set { bluePlayer = value; OnPropertyChanged(nameof(BluePlayer)); }
        }



        #endregion


        #region AddGame
        private Game currGame;

        public Game CurrGame
        {
            get { return currGame; }
            set { currGame = value; OnPropertyChanged(nameof(CurrGame)); }
        }


        #endregion

        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }

        public void Save()
        {
            try
            {
                redPlayer.Game_Name = currGame.Game_Name;
                bluePlayer.Game_Name=currGame.Game_Name;
                greenPlayer.Game_Name= currGame.Game_Name;
                yellowPlayer.Game_Name = currGame.Game_Name;

                var isSaved = ObjGameService.add(currGame);   
                //service to add players
                //Service to add game pieces
            }
            catch (Exception ex)
            {

            }
        }
    }
}
