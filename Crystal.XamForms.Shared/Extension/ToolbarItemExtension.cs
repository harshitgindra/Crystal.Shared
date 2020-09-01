using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class ToolbarItemExtension
    {
        public static ToolbarItem BindCommand(this ToolbarItem self, string path,
            BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind(self, MenuItem.CommandProperty, path, mode, converter, stringFormat);
        }
    }
}