using System;
using System.Windows.Input;

namespace UtilityBelt.Commands
{
    public class MoreAppsCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AppHelper.ShowAuthorMarketplace();
        }

        public event EventHandler CanExecuteChanged;
    }
}
