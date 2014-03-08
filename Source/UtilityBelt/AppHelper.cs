using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml.Linq;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using UtilityBelt.Model;

namespace UtilityBelt
{
    public static class AppHelper
    {
        public static void Reset()
        {
            isBlackTheme = null;
        }

        public static bool IsBlackTheme
        {
            get
            {
                if (!isBlackTheme.HasValue)
                {
                    SolidColorBrush bg = Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;
                    isBlackTheme = bg != null && bg.Color == Colors.Black;
                }
                return isBlackTheme.Value;
            }
        }
        private static bool? isBlackTheme;

        /// <summary>
        /// Application version
        /// </summary>
        public static readonly string Version;

        /// <summary>
        /// Application ID
        /// </summary>
        public static string AppID
        {
            get
            {
                if (appID == null)
                {
                    try
                    {
                        string val = XElement.Load("WMAppManifest.xml").Descendants("App").Single().Attribute("ProductID").Value;
                        appID = Regex.Match(val, "(?<={).*(?=})").Value;
                    }
                    catch (Exception e)
                    {
                    }
                    appID = appID ?? string.Empty;
                }
                return appID;
            }
        }
        private static string appID;

        /// <summary>
        /// Flag indicating if game is running in trial mode or not.
        /// </summary>
        public static bool IsTrial
        {
            get
            {
                if (!isTrial.HasValue) isTrial = Guide.IsTrialMode;
                return isTrial.Value;
            }
        }
        private static bool? isTrial;

        static AppHelper()
        {
            string assembly = Assembly.GetCallingAssembly().FullName;
            Version = assembly.Split('=')[1].Split(',')[0];
        }

        #region Common tasks

        public static void ShareLink(Uri uri, string title, string message)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask
            {
                LinkUri = uri,
                Title = title,
                Message = message,
            };
            SafeShow(shareLinkTask.Show);
        }

        public static void Rate()
        {
            MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            SafeShow(reviewTask.Show);
        }

        public static void SendSMS(string number, string message)
        {
            SmsComposeTask composeSMS = new SmsComposeTask
            {
                Body = message,
                To = number
            };
            SafeShow(composeSMS.Show);
        }

        public static void SendEmail(string email, string subject, string message)
        {
            EmailComposeTask composeEmail = new EmailComposeTask
            {
                To = email,
                Subject = subject,
                Body = message,
            };
            SafeShow(composeEmail.Show);
        }

        public static void ShowWeb(string address)
        {
            if (!address.StartsWith("http://")) address = "http://" + address;
            Uri uri;
            if (!Uri.TryCreate(address, UriKind.Absolute, out uri)) return;
            WebBrowserTask browseWeb = new WebBrowserTask
            {
                Uri = uri
            };
            SafeShow(browseWeb.Show);
        }

        public static void ShowAppMarketplace()
        {
            MarketplaceDetailTask marketplaceDetail = new MarketplaceDetailTask
            {
                ContentType = MarketplaceContentType.Applications
            };
            SafeShow(marketplaceDetail.Show);
        }

        public static void ShowAuthorMarketplace()
        {
            MarketplaceSearchTask marketplaceSearch = new MarketplaceSearchTask
            {
                SearchTerms = Info.AuthorName
            };
            SafeShow(marketplaceSearch.Show);
        }

        #endregion

        #region Safe Navigation methods

        public static void SafeGoBack(this NavigationService service)
        {
            SafeShow(service.GoBack);
        }

        public static void SafeNavigate(this NavigationService service, Uri uri)
        {
            SafeShow(() => service.Navigate(uri));
        }

        #endregion

        public static void SafeShow(Action action)
        {
            try
            {
                action();
            }
            catch (InvalidOperationException e)
            {
                // TODO log
                // catch possible Navigation exception for doubletaps
            }
        }
    }
}
