using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class BaseExtension
    {
        public static TItem Bind<TItem>(View self, BindableProperty targetProperty, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null) where TItem : class
        {
            var binding = new Binding(path, mode, converter, stringFormat: stringFormat);
            self.SetBinding(targetProperty, binding);
            return self as TItem;
        }
    }
}