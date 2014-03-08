using System;
using System.Windows.Input;
using UtilityBelt.Model;
using UtilityBelt.Resources;

namespace UtilityBelt.Commands
{
    public class ShareAppCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AppHelper.ShareLink(new Uri(Info.AppLink), Info.AppName, BeltResources.ShareMessage);
        }

        public event EventHandler CanExecuteChanged;
    }
}
