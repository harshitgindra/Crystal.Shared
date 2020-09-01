using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class LabelExtension
    {
        public static Label BindText(this Label self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind(self, Label.TextProperty, path, mode, converter, stringFormat);
        }

        public static Label BindTextColor(this Label self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind(self, Label.TextColorProperty, path, mode, converter, stringFormat);
        }

        public static Label BindBackgroundColor(this Label self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind(self, VisualElement.BackgroundColorProperty, path, mode, converter,
                stringFormat);
        }

        public static Label BindIsVisible(this Label self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind(self, VisualElement.IsVisibleProperty, path, mode, converter,
                stringFormat);
        }

        public static Label SetFontSize(this Label self, NamedSize namedSize)
        {
            self.FontSize = Device.GetNamedSize(namedSize, typeof(Label));
            return self;
        }

        public static Label SetFontFamily(this Label self, string fontFamily)
        {
            self.FontFamily = fontFamily;
            return self;
        }
    }
}