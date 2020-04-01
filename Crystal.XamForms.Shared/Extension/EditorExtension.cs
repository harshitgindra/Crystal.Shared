using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class EditorExtension
    {
        public static Editor BindText(this Editor self,
            string path,
            BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<Editor>(self, Editor.TextProperty, path, mode, converter, stringFormat);
        }
    }
}