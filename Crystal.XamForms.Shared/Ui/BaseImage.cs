using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseImage : Image
    {
        public BaseImage(IThemeProperty themeProperty, double widthRequest = default, ImageSource source = default) :
            this(widthRequest, source)
        {
            this.BackgroundColor = themeProperty.BackgroundColor;
        }

        public BaseImage(double widthRequest, ImageSource source = null) : base()
        {
            if (widthRequest != default)
            {
                this.WidthRequest = widthRequest;
            }

            if (source != default)
            {
                this.Source = source;
            }
        }
    }
}