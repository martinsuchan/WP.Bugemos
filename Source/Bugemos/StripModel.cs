using System.Collections.ObjectModel;
using UtilityBelt.ViewModel;

namespace Bugemos
{
    public class StripModel : BaseViewModel
    {
        private static StripModel instance;

        public static StripModel Instance
        {
            get { return instance ?? (instance = new StripModel()); }
        }

        public bool Downloading
        {
            get { return downloading; }
            set
            {
                downloading = value;
                NotifyPropertyChanged("Downloading");
            }
        }
        private static bool downloading;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged("Message");
            }
        }
        private static string message;

        public ObservableCollection<Strip> Strips
        {
            get { return Settings.Strips.Value; }
        }

        private StripModel()
        {
        }
    }
}
