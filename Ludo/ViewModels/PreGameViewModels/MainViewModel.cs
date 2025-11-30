using LudoClient.Stores;
using LudoClient.ViewModels.Base;

namespace LudoClient.ViewModels.PreGameViewModels
{
    //MainWindows viewmodel
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel; //get viewmodel from navstore

        public MainViewModel(NavigationStore navigationStore)  
        {
            _navigationStore = navigationStore;
            //MainViewModel subscribed to the navStor event: “When NavigationStore fires its event, call MY method.”
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged; 
        }
        private void OnCurrentViewModelChanged()
        {
            //OnPropertyChanged tells WPF:“The CurrentViewModel property changed — redraw the screen.”
            OnPropertyChanged(nameof(CurrentViewModel)); //implemented on the viewModelBase which will invoke property changed
        }

    }
}
