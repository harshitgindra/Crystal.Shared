using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseButton: Button
    {
        public BaseButton(IThemeProperty themeProperty, 
            string buttonText, 
            Thickness padding = default,
            Thickness margin = default): this(buttonText, padding, margin)
        {
            this.BackgroundColor = themeProperty.TextColor;
            this.BorderColor = themeProperty.BackgroundColor;
            this.TextColor = themeProperty.BackgroundColor;
            this.FontFamily = themeProperty.FontFamily;
        } 
        
        public BaseButton(string text, Thickness padding = default,
            Thickness margin = default):this()
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Text = text;
            }
            if (padding != default)
            {
                this.Padding = padding;
            }
            
            if (margin != default)
            {
                this.Margin = margin;
            }
        } 
        
        public BaseButton()
        {
            this.CornerRadius = 5;
        } 
    }
}