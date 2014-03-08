using System;
using System.Windows.Input;

namespace UtilityBelt.Commands
{
    public class BuyAppCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AppHelper.ShowAppMarketplace();
        }

        public event EventHandler CanExecuteChanged;
    }
}
