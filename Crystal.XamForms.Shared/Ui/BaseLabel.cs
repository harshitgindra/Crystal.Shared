using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseLabel: Label
    {
        public BaseLabel(IThemeProperty themeProperty, 
            string text = null,
            NamedSize fontSize = NamedSize.Default,
            TextAlignment horizontalTextAlignment = TextAlignment.Start,
            Thickness padding = default,
            Thickness margin = default,
            FontAttributes fontAttributes = default,
            TextAlignment verticalTextAlignment = TextAlignment.Start): 
            this(text, fontSize , horizontalTextAlignment, padding, margin, fontAttributes, verticalTextAlignment)
        {
            this.TextColor = themeProperty.TextColor;
            this.BackgroundColor = themeProperty.BackgroundColor;
            this.FontFamily = themeProperty.FontFamily;
        } 
        
        public BaseLabel(): base()
        {
            
        }

        public BaseLabel(string text = null,
            NamedSize fontSize = NamedSize.Default,
            TextAlignment horizontalTextAlignment = TextAlignment.Start,
            Thickness padding = default,
            Thickness margin = default,
            FontAttributes fontAttributes = default,
            TextAlignment verticalTextAlignment = TextAlignment.Start): this()
        {
            this.Text = text;
            this.FontSize = Device.GetNamedSize(fontSize, typeof(Label));
            this.HorizontalTextAlignment = horizontalTextAlignment;
            
            if (padding != default)
            {
                this.Padding = padding;
            }
            
            if (margin != default)
            {
                this.Margin = margin;
            }

            if (fontAttributes != default)
            {
                this.FontAttributes = fontAttributes;
            }
            
            this.VerticalTextAlignment = verticalTextAlignment;
        }
    }
}