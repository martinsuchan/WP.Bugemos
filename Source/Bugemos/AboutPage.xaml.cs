using System;
using System.Windows.Input;
using UtilityBelt;
using UtilityBelt.Model;

namespace Bugemos
{
    public partial class AboutPage
    {
        public AboutPage()
        {
            InitializeComponent();

            DataContext = new Info();
        }

        private void BackClick(object sender, EventArgs e)
        {
            NavigationService.SafeGoBack();
        }

        private void AuthorEmailMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppHelper.SendEmail(Info.AuthorEmail, Info.AppName, "Thanks for this cool app!\nSincerely ...");
        }

        private void AuthorTwitterMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppHelper.ShowWeb(Info.AuthorTwitter);
        }

        private void AuthorMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppHelper.ShowAuthorMarketplace();
        }

        private void AuthorComicsMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppHelper.ShowWeb(App.BugemosHome);
        }

        private void AppMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppHelper.ShowAppMarketplace();
        }
    }
}
