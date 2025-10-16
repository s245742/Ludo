using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; } //Determines our current viewmodel (remember when on homeview also on homviewmodel

        public MainViewModel() 
        {
            CurrentViewModel = new GameViewModel(); //inital viewmodel
        }

    }
}
