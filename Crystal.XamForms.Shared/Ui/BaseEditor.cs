using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseEditor : Editor
    {
        public BaseEditor(IThemeProperty themeProperty,
            string text = default,
            double fontSize = default,
            string placeHolder = default,
            Thickness margin = default) : this(text, fontSize, placeHolder, margin)
        {
            BackgroundColor = themeProperty.BackgroundColor;
            FontFamily = themeProperty.FontFamily;
            TextColor = themeProperty.TextColor;
        }

        public BaseEditor(string text = default,
            double fontSize = default,
            string placeHolder = default,
            Thickness margin = default) : this()
        {
            if (!string.IsNullOrEmpty(text)) Text = text;

            if (fontSize != default) FontSize = fontSize;

            if (placeHolder != default) Placeholder = placeHolder;

            if (margin != default) Margin = margin;
        }

        public BaseEditor()
        {
        }
    }
}