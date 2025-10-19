using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ludo.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action DoWork; //delegate store ref of method that is called by user
        public RelayCommand(Action work) 
        {
            DoWork = work;
        }
        public bool CanExecute(object? parameter)
        {
            return true; //btn enabled, can be clicked
        }

        //Exceuted code when btn pressed
        public void Execute(object? parameter)
        {
            DoWork();
        }
    }
    //Generic version for when the methods use patameters like delte
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action<T> DoWork; //delegate store ref of method that is called by user
        public RelayCommand(Action<T> work)
        {
            DoWork = work;
        }
        public bool CanExecute(object? parameter)
        {
            return true; //btn enabled, can be clicked
        }

        //Exceuted code when btn pressed
        public void Execute(object? parameter)
        {
            //Casts object? to T
            DoWork((T)parameter!);
        }
    }
}
