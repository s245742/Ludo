using Ludo.Stores;
using Ludo.ViewModels.Base;

namespace Ludo.ViewModels.PreGameViewModels
{
    //MainWindows viewmodel
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel; //get viewmodel from navstore

        public MainViewModel(NavigationStore navigationStore)  
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel)); //implemented on the viewModelBase which will invoke property changed
        }

    }
}
