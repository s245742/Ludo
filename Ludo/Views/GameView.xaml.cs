
using LudoClient.ViewModels.InGameViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace Ludo.Views
{
    
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();



        }
        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is GameViewModel vm)
            {
                await vm.InitializeAsync();
            }
        }
    }
}
