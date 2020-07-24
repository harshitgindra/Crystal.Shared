using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class EntryExtension
    {
        public static Entry BindText(this Entry self,
            string path,
            BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<Entry>(self, Entry.TextProperty, path, mode, converter, stringFormat);
        }
    }
}