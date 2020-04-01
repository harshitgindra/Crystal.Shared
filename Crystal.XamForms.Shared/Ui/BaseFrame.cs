using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseFrame: Frame
    {
        public BaseFrame(IThemeProperty themeProperty, View content, Thickness padding = default,
            Thickness margin = default): this(content, padding, margin)
        {
            this.BackgroundColor = themeProperty.BackgroundColor;
            this.BorderColor = themeProperty.TextColor;
        } 
        
        public BaseFrame(View content, Thickness padding = default,
            Thickness margin = default): base()
        {
            this.Content = content;
            if (padding != default)
            {
                this.Padding = padding;
            }
            
            if (margin != default)
            {
                this.Margin = margin;
            }
        } 
    }
}