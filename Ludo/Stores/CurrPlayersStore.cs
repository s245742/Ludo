using LudoClient.Services;
using LudoClient.ViewModels.Base;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoClient.Stores
{
    
    public class CurrPlayersStore : ViewModelBase
    {
        //enables us to transfer our currents games player from joingameviewmodel
        //to gameviewmodel and still keeping viewodels as singleton
        private ObservableCollection<Player> _gamePlayers;
        public ObservableCollection<Player> GamePlayers
        {
            get { return _gamePlayers; }
            set { _gamePlayers = value; OnPropertyChanged(nameof(GamePlayers)); }
        }
        public void SetPlayers(ObservableCollection<Player> players)
        {
            GamePlayers = players;
        }
    }
}
