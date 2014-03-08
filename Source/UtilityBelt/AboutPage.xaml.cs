using System.Windows.Input;
using UtilityBelt.ViewModel;

namespace UtilityBelt
{
    public partial class AboutPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        public AboutPageViewModel Model
        {
            get { return (AboutPageViewModel) DataContext; }
        }

        private void NameTextBoxTap(object sender, GestureEventArgs e)
        {
            Model.BuyCmd.Execute(null);
        }

        private void TwitterBoxTap(object sender, GestureEventArgs e)
        {
            Model.ShowTwitterCmd.Execute(null);
        }

        private void EmailBoxTap(object sender, GestureEventArgs e)
        {
            Model.FeedbackCmd.Execute(null);
        }
    }
}
