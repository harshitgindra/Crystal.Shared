using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class ButtonExtension
    {
        public static Button BindCommand(this Button self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind(self, Button.CommandProperty, path, mode, converter, stringFormat);
        }
    }
}