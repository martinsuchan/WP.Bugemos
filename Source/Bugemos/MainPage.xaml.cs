using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using UtilityBelt;

namespace Bugemos
{
    public partial class MainPage
    {
        private bool initialDownload = true;

        public MainPage()
        {
            InitializeComponent();

            DataContext = StripModel.Instance;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if (initialDownload)
            {
                initialDownload = false;
                lock (this)
                {
                    LoadStripsFromRSSAsync();
                }
            }
        }

        private static async void LoadStripsFromRSSAsync()
        {
            await StripDownloader.LoadRSSAsync(App.BugemosRSS);
        }

        private void StripTap(object sender, GestureEventArgs e)
        {
            Strip strip = ((StackPanel) sender).DataContext as Strip;
            if (strip == null)
            {
                return;
            }

            NavigationService.SafeNavigate(new Uri(string.Format("/ImagePage.xaml?id={0}", strip.Id), UriKind.Relative));
        }

        private void UrlClick(object sender, EventArgs e)
        {
            AppHelper.ShowWeb(App.BugemosHome);
        }

        private void AboutClick(object sender, EventArgs e)
        {
            NavigationService.SafeNavigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}
