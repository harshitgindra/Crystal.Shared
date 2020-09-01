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
            return BaseExtension.Bind(self, Entry.TextProperty, path, mode, converter, stringFormat);
        }

        public static Entry SetFontSize(this Entry self, NamedSize namedSize)
        {
            self.FontSize = Device.GetNamedSize(namedSize, typeof(Label));
            return self;
        }
    }
}