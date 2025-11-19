
drop table Game_Piece;
drop table Player;
drop table Game;
GO

CREATE TABLE Game
(
    Game_Name VARCHAR(50) PRIMARY KEY,
);
GO
CREATE TABLE Player 
(
    ID INT PRIMARY KEY IDENTITY,
    Game_Name VARCHAR(50) NOT NULL,
    Player_Name VARCHAR(50) NOT NULL,
    Color INT NOT NULL,
    FOREIGN KEY (GAME_NAME) REFERENCES Game(Game_Name)
);
GO

Create table Game_Piece
(
    GamePiece_ID INT PRIMARY KEY IDENTITY,
    Player_ID INT,
    SpaceIndex INT,
    SlotIndex INT,
    FOREIGN KEY (Player_ID) REFERENCES Player(ID)
);
GO