using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ludo.Models;
using Ludo.Services;
using Ludo.Commands;
namespace Ludo.ViewModels
{
    public class GameViewModel : ViewModelBase
    {


        GameService ObjGameService;
        public GameViewModel()
        {
            ObjGameService = new GameService(); //can now call method
            LoadData(); //helper method
            CurrGame = new Game();
            saveCommand = new RelayCommand(Save);
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

        #region AddGame
        private Game currGame;

        public Game CurrGame
        {
            get { return currGame; }
            set { currGame = value; OnPropertyChanged("CurrGame"); }
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
                currGame.Name = "Martin3"; //change to test
                var isSaved = ObjGameService.add(currGame);
                LoadData(); //reload display
            }
            catch (Exception ex)
            {

            }
        }
    }
}
