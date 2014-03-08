using System;
using System.Windows.Input;

namespace UtilityBelt.Commands
{
    public class RateAppCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AppHelper.Rate();
        }

        public event EventHandler CanExecuteChanged;
    }
}
