using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models.Cells
{

    public class EmptyCell : Cell
    {
        public EmptyCell(int cellIndex) : base(cellIndex, CellType.Empty)
        {
        }
    }

}
