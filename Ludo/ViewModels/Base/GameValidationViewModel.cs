using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using LudoClient.Services;
using LudoClient.Models;
using LudoClient.ViewModels.Base;
using SharedModels.Models;

public abstract class GameValidationViewModel : ViewModelBase, IDataErrorInfo
{

    #region Properties

    //game
    public string GameName
    {
        get => CurrGame?.Game_Name;
        set
        {
            if (CurrGame != null) CurrGame.Game_Name = value;
            OnPropertyChanged(nameof(GameName));
        }
    }

    // Player 
    public string RedPlayerName
    {
        get => GamePlayers[0].Player_Name;
        set
        {
            GamePlayers[0].Player_Name = value; OnPropertyChanged(nameof(RedPlayerName));
        }
    }
    public string GreenPlayerName
    {
        get => GamePlayers[1].Player_Name;
        set
        {
            GamePlayers[1].Player_Name = value; OnPropertyChanged(nameof(GreenPlayerName));
        }
    }
    public string YellowPlayerName
    {
        get => GamePlayers[2].Player_Name;
        set
        {
            GamePlayers[2].Player_Name = value; OnPropertyChanged(nameof(YellowPlayerName));
        }
    }
    public string BluePlayerName
    {
        get => GamePlayers[3].Player_Name;
        set
        {
            GamePlayers[3].Player_Name = value; OnPropertyChanged(nameof(BluePlayerName));
        }
    }
    #endregion

    public GameValidationViewModel()
    {
        CurrGame = new Game();
        GamePlayers = new ObservableCollection<Player>();
    }

    public Game CurrGame { get; set; }
    public ObservableCollection<Player> GamePlayers { get; set; }
    

    //IDataErrorInfo Check validation
    public string this[string propertyName]
    {
        get
        {
            switch (propertyName)
            {
                case nameof(GameName):
                    if (string.IsNullOrWhiteSpace(GameName))
                        return "Game name is required";
                    break;
                case nameof(RedPlayerName):
                    if (string.IsNullOrWhiteSpace(RedPlayerName))
                        return "Red player name is required";
                    break;
                case nameof(GreenPlayerName):
                    if (string.IsNullOrWhiteSpace(GreenPlayerName))
                        return "Green player name is required";
                    break;
                case nameof(YellowPlayerName):
                    if (string.IsNullOrWhiteSpace(YellowPlayerName))
                        return "Yellow player name is required";
                    break;
                case nameof(BluePlayerName):
                    if (string.IsNullOrWhiteSpace(BluePlayerName))
                        return "Blue player name is required";
                    break;
            }
            return null;
        }
    }

    public string Error => null;

    //Get all errors store inIEnumberalbel

    
    public IEnumerable<string> GetAllErrors()
    {
        var errors = new List<string>();
        if (!string.IsNullOrWhiteSpace(this[nameof(GameName)]))
            errors.Add(this[nameof(GameName)]);
        if (!string.IsNullOrWhiteSpace(this[nameof(RedPlayerName)]))
            errors.Add(this[nameof(RedPlayerName)]);
        if (!string.IsNullOrWhiteSpace(this[nameof(GreenPlayerName)]))
            errors.Add(this[nameof(GreenPlayerName)]);
        if (!string.IsNullOrWhiteSpace(this[nameof(YellowPlayerName)]))
            errors.Add(this[nameof(YellowPlayerName)]);
        if (!string.IsNullOrWhiteSpace(this[nameof(BluePlayerName)]))
            errors.Add(this[nameof(BluePlayerName)]);
        return errors;
    }

    public bool CanSave() => !GetAllErrors().Any();
}