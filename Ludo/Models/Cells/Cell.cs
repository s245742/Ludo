using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public abstract class Cell
    {
        public int CellIndex { get;}
        public CellType CellType;        
        
        public ObservableCollection<Piece> Pieces { get; set; } = new ObservableCollection<Piece>();

        public Cell(int cellIndex, CellType type)
        {
            CellIndex = cellIndex;
            CellType = type;
            ;
        }
    }
}
