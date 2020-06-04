using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseToolbarItem : ToolbarItem
    {
        public BaseToolbarItem(IThemeProperty themeProperty,
            ImageSource imageSource = default,
            int priority = default,
            string text = default,
            Command command = default)
        {
            IconImageSource = imageSource;
            Priority = priority;
            Text = text;
            Command = command;
        }
    }
}