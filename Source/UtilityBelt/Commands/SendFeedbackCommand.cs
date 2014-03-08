using System;
using System.Windows.Input;
using UtilityBelt.Resources;
using UtilityBelt.ViewModel;

namespace UtilityBelt.Commands
{
    public class SendFeedbackCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AppHelper.SendEmail(AboutPageViewModel.AuthorEmail, AboutPageViewModel.AppName, BeltResources.FeedbackMessage);
        }

        public event EventHandler CanExecuteChanged;
    }
}
