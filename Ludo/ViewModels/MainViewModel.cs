using Ludo.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.ViewModels
{
    //used to manage the current view and viewmodel which is stored in navgiationstore
    // and is used in "app.xaml.cs" and in mainwindow.xaml"
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;

        public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel; //get viewmodel from navstore

        public MainViewModel(NavigationStore navigationStore)  
        {
            this.navigationStore = navigationStore;
            navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel)); //implemented on the viewModelBase which will invoke property changed
        }

    }
}
