using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseStackLayout : StackLayout
    {
        public BaseStackLayout(IThemeProperty themeProperty, Thickness padding = default,
            Thickness margin = default,
            View[] children = default) : this(padding, margin, children)
        {
            this.BackgroundColor = themeProperty.BackgroundColor;
        }

        public BaseStackLayout(Thickness padding = default,
            Thickness margin = default,
            View[] children = default) : base()
        {
            if (padding != default)
            {
                this.Padding = padding;
            }

            if (margin != default)
            {
                this.Margin = margin;
            }

            if (children != default)
            {
                foreach (View child in children)
                {
                    this.Children.Add(child);
                }
            }
        }
    }
}