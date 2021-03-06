using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class ImageExtension
    {
        public static Image BindImageSource(this Image self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind(self, Image.SourceProperty, path, mode, converter, stringFormat);
        }

        public static Image SetAspect(this Image self, Aspect aspect)
        {
            self.Aspect = aspect;
            return self;
        }
        }
}