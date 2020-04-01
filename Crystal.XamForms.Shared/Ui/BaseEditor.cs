using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseEditor: Editor
    {
        public BaseEditor(IThemeProperty themeProperty, 
            string text = default,
            double fontSize = default,
            string placeHolder = default,
            Thickness margin = default): this(text, fontSize, placeHolder, margin)
        {
            this.BackgroundColor = themeProperty.BackgroundColor;
            this.FontFamily = themeProperty.FontFamily;
            this.TextColor = themeProperty.TextColor;
        } 
        
        public BaseEditor(string text = default,
            double fontSize = default,
            string placeHolder = default,
            Thickness margin = default):this()
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Text = text;
            }
            if (fontSize != default)
            {
                this.FontSize = fontSize;
            }
            
            if (placeHolder != default)
            {
                this.Placeholder = placeHolder;
            }
            
            if (margin != default)
            {
                this.Margin = margin;
            }
        } 
        
        public BaseEditor()
        {
        } 
    }
}