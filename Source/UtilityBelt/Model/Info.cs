namespace UtilityBelt.Model
{
    public class Info
    {
        #region Application dependant properties

        public static string AppName
        {
            get { return appName; }
            set { appName = value; }
        }
        private static string appName = "{app name}";

        public static string AppDescription
        {
            get { return appDescription; }
            set { appDescription = value; }
        }
        private static string appDescription = "{App description. Useful tool containing all ingredients with all effects available in The Elder Scrolls V: Skyrim.}";

        public static string AppWeb
        {
            get { return appWeb; }
            set { appWeb = value; }
        }
        private static string appWeb = "www.suchan.cz";

        public static string AppEmail
        {
            get { return appEmail; }
            set { appEmail = value; }
        }
        private static string appEmail = "martin@suchan.cz";

        #endregion

        public static string AppVersion
        {
            get { return AppHelper.Version.Substring(0, 3); }
        }

        public static string AppLink
        {
            get { return string.Format("http://windowsphone.com/s?appId={0}", AppHelper.AppID); }
        }

        public static string AuthorName
        {
            get { return "Martin Suchan"; }
        }

        public static string AuthorEmail
        {
            get { return "martin@suchan.cz"; }
        }

        public static string AuthorWeb
        {
            get { return "www.suchan.cz"; }
        }

        public static string AuthorTwitter
        {
            get { return "twitter.com/martinsuchan"; }
        }
    }
}
