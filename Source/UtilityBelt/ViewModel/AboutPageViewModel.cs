using System.Windows;
using System.Windows.Input;
using UtilityBelt.Commands;
using UtilityBelt.Model;
using UtilityBelt.Resources;

namespace UtilityBelt.ViewModel
{
    public class AboutPageViewModel
    {
        #region Info

        public static string AppName
        {
            get { return Info.AppName; }
        }

        public static string AppDescription
        {
            get { return Info.AppDescription; }
        }

        public static string AppVersion
        {
            get { return Info.AppVersion; }
        }

        public static string AuthorName
        {
            get { return Info.AuthorName; }
        }

        public static string AuthorEmail
        {
            get { return Info.AuthorEmail; }
        }

        public static string AuthorTwitter
        {
            get { return Info.AuthorTwitter; }
        }

        public static Visibility IsTrial
        {
            get { return AppHelper.IsTrial ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region Commands

        public ICommand BuyCmd
        {
            get { return buyCmd ?? (buyCmd = new BuyAppCommand()); }
        }
        private ICommand buyCmd;

        public ICommand RateCmd
        {
            get { return rateCmd ?? (rateCmd = new RateAppCommand()); }
        }
        private ICommand rateCmd;

        public ICommand ShareCmd
        {
            get { return shareCmd ?? (shareCmd = new ShareAppCommand()); }
        }
        private ICommand shareCmd;

        public ICommand MoreAppsCmd
        {
            get { return moreAppsCmd ?? (moreAppsCmd = new MoreAppsCommand()); }
        }
        private ICommand moreAppsCmd;

        public ICommand ShowTwitterCmd
        {
            get { return showTwitterCmd ?? (showTwitterCmd = new ShowTwitterCommand()); }
        }
        private ICommand showTwitterCmd;

        public ICommand FeedbackCmd
        {
            get { return feedbackCmd ?? (feedbackCmd = new SendFeedbackCommand()); }
        }
        private ICommand feedbackCmd;

        #endregion

        public BeltResources Loc
        {
            get { return loc; }
        }
        private static readonly BeltResources loc = new BeltResources();
    }
}
