using System;
using System.Diagnostics;
using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Behavior
{
    public class SwipeGesture : PanGestureRecognizer
    {
        private readonly ISwipeCallBack _mISwipeCallback;
        private double _translatedX, _translatedY;

        public SwipeGesture(View view, ISwipeCallBack iSwipeCallBack)
        {
            _mISwipeCallback = iSwipeCallBack;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(panGesture);
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var content = (View)sender;

            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    try
                    {
                        _translatedX = e.TotalX;
                        _translatedY = e.TotalY;
                    }
                    catch (Exception err)
                    {
                        Debug.WriteLine("" + err.Message);
                    }

                    break;

                case GestureStatus.Completed:

                    if (_translatedX < 0 && Math.Abs(_translatedX) > Math.Abs(_translatedY))
                        _mISwipeCallback.OnLeftSwipe(content);
                    else if (_translatedX > 0 && _translatedX > Math.Abs(_translatedY))
                        _mISwipeCallback.OnRightSwipe(content);
                    else if (_translatedY < 0 && Math.Abs(_translatedY) > Math.Abs(_translatedX))
                        _mISwipeCallback.OnTopSwipe(content);
                    else if (_translatedY > 0 && _translatedY > Math.Abs(_translatedX))
                        _mISwipeCallback.OnBottomSwipe(content);
                    else
                        _mISwipeCallback.OnNothingSwiped(content);

                    break;
            }
        }
    }
}