using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using UtilityBelt;

namespace Bugemos
{
    public partial class ImagePage
    {
        private Strip strip;
        private int nextId;
        private int prevId;

        public ImagePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (strip != null) return;

            string idstr;
            if (NavigationContext.QueryString.TryGetValue("id", out idstr))
            {
                int id;
                if (int.TryParse(idstr, out id))
                {
                    LoadStrip(id);
                }
            }
        }

        private void LoadStrip(int id)
        {
            DataContext = strip = StripModel.Instance.Strips.First(s => s.Id == id);
            nextId = FindNextStrip(strip.Id);
            prevId = FindPrevStrip(strip.Id);
            ApplicationBarIconButton next = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            next.IsEnabled = nextId > -1;
            ApplicationBarIconButton prev = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            prev.IsEnabled = prevId > -1;
            MainImage.RenderTransform = transform = new CompositeTransform();
        }

        private double initialScale;

        private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            initialScale = transform.ScaleX;
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            double ratio = initialScale*e.DistanceRatio;
            ratio = Math.Max(0.75, ratio);
            ratio = Math.Min(6, ratio);
            transform.ScaleX = transform.ScaleY = ratio;

            NormalizePosition(0, 0);
        }

        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            NormalizePosition(e.HorizontalChange, e.VerticalChange);
        }

        private void NormalizePosition(double horizontalChange, double verticalChange)
        {
            double actImgWidth = MainImage.ActualWidth * transform.ScaleX;
            double wpad = Math.Abs((ActualWidth - actImgWidth) / 2);

            double movex = transform.TranslateX + horizontalChange;
            movex = Math.Min(wpad, movex);
            movex = Math.Max(-wpad, movex);
            transform.TranslateX = movex;

            double actImgHeight = MainImage.ActualHeight * transform.ScaleY;
            double hpad = Math.Abs((actImgHeight - ActualHeight) / 2);

            double movey = transform.TranslateY + verticalChange;
            movey = Math.Min(hpad, movey);
            movey = Math.Max(-hpad, movey);
            transform.TranslateY = movey;
        }

        protected virtual void PrevClick(object sender, EventArgs e)
        {
            if (prevId > -1)
            {
                LoadStrip(prevId);
            }
        }

        protected virtual void NextClick(object sender, EventArgs e)
        {
            if (nextId > -1)
            {
                LoadStrip(nextId);
            }
        }

        private void ShareClick(object sender, EventArgs e)
        {
            // TODO
        }

        private void WebClick(object sender, EventArgs e)
        {
            if (strip.Link != null)
            {
                AppHelper.ShowWeb(strip.Link);
            }
        }

        private int FindNextStrip(int id)
        {
            ObservableCollection<Strip> list = StripModel.Instance.Strips;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Id > id)
                {
                    return list[i].Id;
                }

            }
            // fallback
            return -1;
        }

        private int FindPrevStrip(int id)
        {
            ObservableCollection<Strip> list = StripModel.Instance.Strips;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id < id)
                {
                    return list[i].Id;
                }

            }
            // fallback
            return -1;
        }
    }
}
