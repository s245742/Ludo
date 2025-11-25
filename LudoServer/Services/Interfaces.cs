using System.Collections.ObjectModel;
using SharedModels.Models;

namespace LudoServer.Services
{
    public interface IGameService
    {
        ObservableCollection<Game> getAll();
        bool CreateGame(Game game);
        bool delete(Game game);
    }

    public interface IPlayerService
    {
        bool CreatePlayer(Player player);
        bool deletePlayersfromGame(Game game);
        ObservableCollection<Player> getAllPlayersFromGame(Game game);
    }

    public interface IGamePieceService
    {
        bool CreateGamePiece(Piece piece);
        bool deleteGamePiecesfromGame(Game game);
        ObservableCollection<Piece> getAllGamePieceFromPlayer(Player player);
        int GetPieceIDFromPiece(Piece piece);
        bool UpdatePieceFromPieceID(Piece pieceToUpdate, int pieceId);
    }
}
