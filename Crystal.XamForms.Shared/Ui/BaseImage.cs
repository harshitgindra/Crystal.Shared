using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseImage : Image
    {
        public BaseImage(IThemeProperty themeProperty, double widthRequest = default, ImageSource source = default) :
            this(widthRequest, source)
        {
            BackgroundColor = themeProperty.BackgroundColor;
        }

        public BaseImage(double widthRequest, ImageSource source = null)
        {
            if (widthRequest != default) WidthRequest = widthRequest;

            if (source != default) Source = source;
        }
    }
}