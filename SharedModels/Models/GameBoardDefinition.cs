using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models
{

    // this class defines the game board layout and starting positions for a Ludo game in the uniform grid system
    public static class GameViewBoardDefinition
    {

        // The main path that pieces follow on the board
        public static readonly int[] Path = new int[]
        {
            6, 7, 8,
            23, 38, 53, 68, 83,
            99, 100, 101, 102, 103, 104,
            119, 134,
            133, 132, 131, 130, 129,
            143, 158, 173, 188, 203, 218,
            217, 216,
            201, 186, 171, 156, 141,
            125, 124, 123, 122, 121, 120,
            105, 90,
            91, 92, 93, 94, 95,
            81, 66, 51, 36, 21,
        };

        // Starting positions for each player's pieces
        public static readonly int[] GreenStart = { 27, 28, 43, 42 };
        public static readonly int[] BlueStart = { 193, 208, 207, 192 };
        public static readonly int[] YellowStart = { 197, 196, 181, 182 };
        public static readonly int[] RedStart = { 31, 16, 17, 32 };

        // Home paths for each color leading to the goal
        public static readonly int[] GreenHome = new[] { 22, 37, 52, 67, 82 };
        public static readonly int[] YellowHome = new[] { 142, 157, 172, 187, 202 }.Reverse().ToArray();
        public static readonly int[] RedHome = new[] { 106, 107, 108, 109, 110 };
        public static readonly int[] BlueHome = new[] { 114, 115, 116, 117, 118 }.Reverse().ToArray();

        // Goal positions for each color
        public static int GreenGoal = 97;
        public static int YellowGoal = 127;
        public static int RedGoal = 111;
        public static int BlueGoal = 113;


        // Home entry positions for each color on the main path
        public static int GreenHomeEntry = 23;
        public static int YellowHomeEntry = 201;
        public static int RedHomeEntry = 91;
        public static int BlueHomeEntry = 133;

        // Globes positions on the main path
        public static int[] Globes = { 23, 51, 91, 122, 133, 158, 201, 216 };

        //Offsets in the Path array for each color's start and home entry positions
        private static int BlueHomeEntryOffset = 18;
        private static int YellowHomeEntryOffset = 33;
        private static int RedHomeEntryOffset = 48;
        private static int GreenHomeEntryOffset = 3;

    }

    public static class GameBoardDefinitions
    {

        // Stars in the Path array
        public static int[] Stars = { 1, 8, 14, 21, 27, 34, 40, 47 };

        public static int[] Globes = { 3, 11, 16, 24, 29, 37, 42, 49 };


        // Offsets in the Path array for each color's start and home entry positions
        public static int GreenHomeEntry = 0;
        public static int BlueHomeEntry = 13;
        public static int YellowHomeEntry = 26;
        public static int RedHomeEntry = 39;


        public static int GreenGoalEntry = 57;
        public static int BlueGoalEntry = 11;
        public static int YellowGoalEntry = 24;
        public static int RedGoalEntry = 37;


        public static readonly Dictionary<PieceColor, int> PathOffsets = new()
    {
        { PieceColor.Green, 3 },
        { PieceColor.Blue, 16 },
        { PieceColor.Yellow, 29 },
        { PieceColor.Red, 42 }
    };


    }

}
