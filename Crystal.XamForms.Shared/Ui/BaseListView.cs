using System;
using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseListView : ListView
    {
        public BaseListView(IThemeProperty themeProperty,
            Type dataTemplateType = null,
            bool isPullDownToRefreshEnabled = false,
            SeparatorVisibility separatorVisibility = SeparatorVisibility.Default,
            View header = null,
            View footer = null,
            Thickness margin = default) :
            this(dataTemplateType, isPullDownToRefreshEnabled, separatorVisibility, header, footer, margin)
        {
            //this.BackgroundColor = themeProperty.BackgroundColor;
        }

        public BaseListView(Type dataTemplateType = null,
            bool isPullDownToRefreshEnabled = false,
            SeparatorVisibility separatorVisibility = SeparatorVisibility.Default,
            View header = null,
            View footer = null,
            Thickness margin = default) : base(ListViewCachingStrategy.RecycleElement)
        {
            HasUnevenRows = true;
            if (dataTemplateType != null) ItemTemplate = new DataTemplate(dataTemplateType);

            IsPullToRefreshEnabled = isPullDownToRefreshEnabled;
            SeparatorVisibility = separatorVisibility;
            Header = header;
            Footer = footer;

            if (margin != default) Margin = margin;
        }
    }
}