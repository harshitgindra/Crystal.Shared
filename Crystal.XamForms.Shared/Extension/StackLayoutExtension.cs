using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class StackLayoutExtension
    {
        public static StackLayout AddChildren(this StackLayout self, params View[] views)
        {
            foreach (var view in views)
            {
                self.Children.Add(view);
            }

            return self;
        }
    }
}
