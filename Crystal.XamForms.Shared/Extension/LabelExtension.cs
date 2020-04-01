using MvvmCross.Forms.Bindings;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class LabelExtension
    {
        public static Label BindText(this Label self, string path, BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<Label>(self, Label.TextProperty, path, mode, converter, stringFormat);
        }
        
        public static Label BindTextColor(this Label self, string path, BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<Label>(self, Label.TextColorProperty, path, mode, converter, stringFormat);
        }
        
        public static Label BindBackgroundColor(this Label self, string path, BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<Label>(self, VisualElement.BackgroundColorProperty, path, mode, converter, stringFormat);
        }
        
        public static Label BindIsVisible(this Label self, string path, BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<Label>(self, VisualElement.IsVisibleProperty, path, mode, converter, stringFormat);
        }
    }
}