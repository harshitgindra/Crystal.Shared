using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseLabel : Label
    {
        public BaseLabel(IThemeProperty themeProperty,
            string text = null,
            NamedSize fontSize = NamedSize.Default,
            TextAlignment horizontalTextAlignment = TextAlignment.Start,
            Thickness padding = default,
            Thickness margin = default,
            FontAttributes fontAttributes = default,
            TextAlignment verticalTextAlignment = TextAlignment.Start) :
            this(text, fontSize, horizontalTextAlignment, padding, margin, fontAttributes, verticalTextAlignment)
        {
            TextColor = themeProperty.TextColor;
            BackgroundColor = themeProperty.BackgroundColor;
            FontFamily = themeProperty.FontFamily;
        }

        public BaseLabel()
        {
        }

        public BaseLabel(string text = null,
            NamedSize fontSize = NamedSize.Default,
            TextAlignment horizontalTextAlignment = TextAlignment.Start,
            Thickness padding = default,
            Thickness margin = default,
            FontAttributes fontAttributes = default,
            TextAlignment verticalTextAlignment = TextAlignment.Start) : this()
        {
            Text = text;
            FontSize = Device.GetNamedSize(fontSize, typeof(Label));
            HorizontalTextAlignment = horizontalTextAlignment;

            if (padding != default) Padding = padding;

            if (margin != default) Margin = margin;

            if (fontAttributes != default) FontAttributes = fontAttributes;

            VerticalTextAlignment = verticalTextAlignment;
        }
    }
}