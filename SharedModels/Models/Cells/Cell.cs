using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.Cells;

public abstract class Cell
{
    public int CellIndex { get;}
    public CellType CellType;  
    public int Size { get; set; } = 0;

    public ObservableCollection<Piece> Pieces { get; set; } = new ObservableCollection<Piece>();

    public Cell(int cellIndex, CellType type)
    {
        CellIndex = cellIndex;
        CellType = type;
        ;
    }

    // Add a public getter for Pieces
    public ObservableCollection<Piece> GetPieces()
    {
        return Pieces;
    }
    // eller public IReadOnlyList<Piece> GetPieces() => Pieces.ToList();
}
