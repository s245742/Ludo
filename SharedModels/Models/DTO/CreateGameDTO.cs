using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.DTO
{
    public class CreateGameDTO
    {
        public Game Game { get; set; }
        public List<Player> Players { get; set; } // note not observableColl since this is used in backend


    }
}
