using Crystal.XamForms.Shared.Behavior;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class ListViewExtension
    {
        public static ListView BindItemSource(this ListView self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<ListView>(self, ListView.ItemsSourceProperty, path, mode, converter,
                stringFormat);
        }

        public static ListView BindSelectedItem(this ListView self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<ListView>(self, ListView.SelectedItemProperty, path, mode, converter,
                stringFormat);
        }

        public static ListView BindRefreshCommand(this ListView self, string path,
            BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<ListView>(self, ListView.RefreshCommandProperty, path, mode, converter,
                stringFormat);
        }

        public static ListView BindIsRefreshing(this ListView self, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind<ListView>(self, ListView.IsRefreshingProperty, path, mode, converter,
                stringFormat);
        }

        public static ListView BindInfiniteScroll(this ListView self, string path,
            BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            var infiniteScroll = new InfiniteScroll();
            infiniteScroll.SetBinding(InfiniteScroll.LoadMoreCommandProperty, path, mode, converter, stringFormat);
            self.Behaviors.Add(infiniteScroll);
            return self;
        }
    }
}