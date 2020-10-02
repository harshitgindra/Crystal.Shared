using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class BaseExtension
    {
        public static dynamic Bind(dynamic self, BindableProperty targetProperty, string path,
            BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            var binding = new Binding(path, mode, converter, stringFormat: stringFormat);
            self.SetBinding(targetProperty, binding);
            return self;
        }

        public static TItem SetMargin<TItem>(this View self, int margin) where TItem : class
        {
            self.Margin = margin;
            return self as TItem;
        }

        public static TItem SetMargin<TItem>(this View self, Thickness margin) where TItem : class
        {
            self.Margin = margin;
            return self as TItem;
        }

        public static TItem SetBackgroundColor<TItem>(this View self, Color backgroundColor) where TItem : class
        {
            self.BackgroundColor = backgroundColor;
            return self as TItem;
        }
    }
}