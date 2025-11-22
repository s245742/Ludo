
using LudoClient.ViewModels.Base;
using LudoClient.ViewModels.InGameViewModels;
using SharedModels.Models;
using SharedModels.Models.Cells;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;

namespace Ludo.ViewModels.InGameViewModels;

public class CellViewModel : ViewModelBase
{
    public int CellIndex { get; }
    public CellType Type { get; }
    public PieceColor? Color { get; }

    public bool IsStar { get; set; }
    public bool IsGlobe { get; set; }

    public ObservableCollection<PieceViewModel> Pieces { get; } = new();

    public PieceColor? OwnedBy { get; set; }

    public Brush GlobeColor => OwnedBy switch
    {
        PieceColor.Yellow => Brushes.LightGoldenrodYellow,
        PieceColor.Red=> Brushes.IndianRed,
        PieceColor.Blue => Brushes.DeepSkyBlue,
        PieceColor.Green => Brushes.DarkOliveGreen,
        
        _ => Brushes.DarkGray
    };

    public Brush CellColor => Type switch
    {
        CellType.Path => Brushes.LightGray,
        CellType.Home => Color switch
        {
            PieceColor.Red => Brushes.LightPink,
            PieceColor.Blue => Brushes.LightBlue,
            PieceColor.Green => Brushes.LightGreen,
            PieceColor.Yellow => Brushes.LightYellow,
            _ => Brushes.Gray
        },
        CellType.Goal => Color switch
        {
            PieceColor.Red => Brushes.Red,
            PieceColor.Blue => Brushes.DodgerBlue,
            PieceColor.Green => Brushes.LimeGreen,
            PieceColor.Yellow => Brushes.Gold,
            _ => Brushes.Gray

        },
        CellType.GoalPath => Color switch
        { 
            PieceColor.Red => Brushes.LightPink,
            PieceColor.Blue => Brushes.LightBlue,
            PieceColor.Green => Brushes.LightGreen,
            PieceColor.Yellow => Brushes.LightGoldenrodYellow,
            _ => Brushes.Gray
        },
        CellType.Empty => Brushes.Transparent,
        _ => Brushes.Transparent
    };

    public CellViewModel(Cell cell, PieceColor color)
    {
        CellIndex = cell.CellIndex;
        Type = cell.CellType;
        if (cell is HomeCell home) Color = home.Color;
        else if (cell is GoalCell goal) Color = goal.Color;
        else if (cell is GoalPathCell goalPath) Color = goalPath.PathColor;
        else if (cell is PathCell pathCell)
        {
            IsStar = GameBoardDefinitions.Stars.Contains(pathCell.PathIndex);
            IsGlobe = GameBoardDefinitions.Globes.Contains(pathCell.PathIndex);
            OwnedBy = color;

        }
    }

}