using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
namespace SharedModels.Models
{
    public class Game 
    {

        private string game_name;

        public string Game_Name 
        {
            get { return game_name; }
            set { game_name = value;}
        }

    }
}
