using System;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using UtilityBelt.Model;
using UtilityBelt.Resources;

namespace UtilityBelt
{
    public class ExcHelper
    {
        public static readonly Setting<string> Error = new Setting<string>("Error", null);

        public static void Handle(Exception ex)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("App: {0} {1}, time: {2}", Info.AppName, Info.AppVersion, DateTime.Now));

                // append exception
                while (ex != null)
                {
                    sb.AppendLine(string.Format("\nException: {0}", ex));
                    ex = ex.InnerException;
                }

                // append settings
                ISettings settings = Context.Get<ISettings>();
                sb.AppendLine(settings.ToString());

                Error.Value = sb.ToString();
                settings.Cleanup();
            }
            catch (Exception exception)
            {
                Error.Value = string.Format("Error when collecting exception info: {0}", exception);
            }
        }

        public static void Show()
        {
            // report saved error if there is any
            if (!string.IsNullOrEmpty(Error.Value))
            {
                Guide.BeginShowMessageBox(BeltResources.ErrorTitle, string.Format(BeltResources.ErrorMessage, Info.AppName),
                    new[] { BeltResources.ErrorReport, BeltResources.ErrorForget }, 0, MessageBoxIcon.Error, ErrorResult, null);
            } 
        }

        private static void ErrorResult(IAsyncResult result)
        {
            int? index = Guide.EndShowMessageBox(result);

            // send email with error message
            if (index.HasValue && index.Value == 0)
            {
                AppHelper.SendEmail(Info.AppEmail, string.Format("[error] {0} {1}", Info.AppName, Info.AppVersion), Error.Value);
            }
            Error.Value = null;
        }
    }
}
