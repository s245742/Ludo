using Ludo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Stores
{
    //Store current application state of viewmodel here 
    public class NavigationStore
    {

        public event Action CurrentViewModelChanged;

        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel //Determines our current viewmodel
        {
            get => currentViewModel; 
            set
            {
                currentViewModel = value;
                OnCurrentViewModelChanged(); //invoke when called
                
            }
        
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke(); //if not null then all subscribers (main viewmodel) to event will be notified
        }



    }
}
