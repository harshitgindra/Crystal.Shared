using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class ToolbarItemExtension 
    {
        public static ToolbarItem BindCommand(this ToolbarItem self, string path, BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return Bind(self, MenuItem.CommandProperty, path, mode, converter, stringFormat);
        }

        private static ToolbarItem Bind(ToolbarItem self, BindableProperty targetProperty, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            var binding = new Binding(path, mode, converter, stringFormat: stringFormat);
            self.SetBinding(targetProperty, binding);
            return self;
        }
    }
}