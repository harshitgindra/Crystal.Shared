using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class GridExtension
    {
        public static Grid AddChildren(this Grid self, params View[] views)
        {
            foreach (var view in views)
            {
                self.Children.Add(view);
            }

            return self;
        }
    }
}