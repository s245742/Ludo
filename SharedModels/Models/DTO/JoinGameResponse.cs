using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.DTO
{
    public class JoinGameResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Player> Players { get; set; }
    }
}
