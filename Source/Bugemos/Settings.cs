using System.Collections.Generic;
using System.Collections.ObjectModel;
using UtilityBelt;

namespace Bugemos
{
    public class Settings : ISettings
    {
        // Persistent user settings from the settings page
        public static readonly Setting<ObservableCollection<Strip>> Strips = new Setting<ObservableCollection<Strip>>("Strips", new ObservableCollection<Strip>());
        public static readonly Setting<List<int>> InvalidStrips = new Setting<List<int>>("InvalidStrips", new List<int>());

        public static readonly Setting<int> LastStrip = new Setting<int>("LastStrip", 0);

        static Settings()
        {
            Context.Set<ISettings>(new Settings());
        }

        #region Overrides of ISettings

        public void Cleanup()
        {
            Strips.Value = new ObservableCollection<Strip>();
        }

        public override string ToString()
        {
            // TODO
            return string.Empty;
        }

        #endregion
    }
}
