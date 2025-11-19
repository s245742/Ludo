using LudoClient.Stores;
using LudoClient.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoClient.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase //Constraint needed
    {
        private readonly NavigationStore navigationStore;
        private readonly Func<TViewModel> createViewModel; //idea is that TViewModel is the type of model we navigate to

        public NavigateCommand(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
        }

        public override void Execute(object? parameter)
        {
            navigationStore.CurrentViewModel = createViewModel(); //this is where the constraint needs to be viewmodelbase
        }
    }
}
