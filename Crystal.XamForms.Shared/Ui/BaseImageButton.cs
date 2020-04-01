using System.Security.Cryptography;
using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseImageButton: ImageButton
    {
        public BaseImageButton(IThemeProperty themeProperty, Thickness margin = default,
            ImageSource imageSource = default,
            Command command = default): this(margin, imageSource, command)
        {
        } 
        
        public BaseImageButton(Thickness margin = default,
            ImageSource imageSource = default,
            Command command = default)
        {
            if (margin != default)
            {
                this.Margin = margin;
            }
            
            if (imageSource != default)
            {
                this.Source = imageSource;
            }
            
            if (command != default)
            {
                this.Command = command;
            }
        }
    }
}