using System;
using System.Diagnostics;
using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Behavior
{
    public class SwipeGesture : PanGestureRecognizer
    {
        private readonly ISwipeCallBack mISwipeCallback;
        private double translatedX, translatedY;

        public SwipeGesture(View view, ISwipeCallBack iSwipeCallBack)
        {
            mISwipeCallback = iSwipeCallBack;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(panGesture);
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var Content = (View) sender;

            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    try
                    {
                        translatedX = e.TotalX;
                        translatedY = e.TotalY;
                    }
                    catch (Exception err)
                    {
                        Debug.WriteLine("" + err.Message);
                    }

                    break;

                case GestureStatus.Completed:

                    Debug.WriteLine("translatedX : " + translatedX);
                    Debug.WriteLine("translatedY : " + translatedY);

                    if (translatedX < 0 && Math.Abs(translatedX) > Math.Abs(translatedY))
                        mISwipeCallback.OnLeftSwipe(Content);
                    else if (translatedX > 0 && translatedX > Math.Abs(translatedY))
                        mISwipeCallback.OnRightSwipe(Content);
                    else if (translatedY < 0 && Math.Abs(translatedY) > Math.Abs(translatedX))
                        mISwipeCallback.OnTopSwipe(Content);
                    else if (translatedY > 0 && translatedY > Math.Abs(translatedX))
                        mISwipeCallback.OnBottomSwipe(Content);
                    else
                        mISwipeCallback.OnNothingSwiped(Content);

                    break;
            }
        }
    }
}