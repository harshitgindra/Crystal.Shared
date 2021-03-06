using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class ViewExtension
    {
        public static BindableObject BindBackgroundColor(this BindableObject self, string path,
            BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return Bind(self, VisualElement.BackgroundColorProperty, path, mode, converter,
                stringFormat);
        }

        public static BindableObject BindTitle(this BindableObject self, string path,
            BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return Bind(self, Xamarin.Forms.Page.TitleProperty, path, mode, converter, stringFormat);
        }

        private static BindableObject Bind(BindableObject self, BindableProperty targetProperty, string path,
            BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            var binding = new Binding(path, mode, converter, stringFormat: stringFormat);
            self.SetBinding(targetProperty, binding);
            return self;
        }

        public static View AddToGrid(this View self, int row, int column)
        {
            Grid.SetColumn(self, column);
            Grid.SetRow(self, row);
            return self;
        }

        public static View SetBackgroundColor(this View self, Color backgroundColor)
        {
            self.BackgroundColor = backgroundColor;
            return self;
        }
    }
}