using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.DTO
{
    public class GameMove
    {
        public Piece Piece { get; set; }    // or color: "Red", "Blue", etc.

        public int FromIndex { get; set; }      // start cell index
        public int ToIndex { get; set; }        // end cell index
    }
}
