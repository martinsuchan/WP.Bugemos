using System;
using System.Windows.Input;
using UtilityBelt.ViewModel;

namespace UtilityBelt.Commands
{
    public class ShowTwitterCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AppHelper.ShowWeb(AboutPageViewModel.AuthorTwitter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
