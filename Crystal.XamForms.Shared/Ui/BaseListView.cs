using System;
using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseListView: ListView
    {
        public BaseListView(IThemeProperty themeProperty, 
            Type dataTemplateType = null, 
            bool isPullDownToRefreshEnabled = false,
            SeparatorVisibility separatorVisibility = SeparatorVisibility.Default,
            View header = null, 
            View footer = null,
            Thickness margin = default):
            this(dataTemplateType, isPullDownToRefreshEnabled, separatorVisibility, header, footer,  margin)
        {
            //this.BackgroundColor = themeProperty.BackgroundColor;
        } 
        
        public BaseListView(Type dataTemplateType = null, 
            bool isPullDownToRefreshEnabled = false,
            SeparatorVisibility separatorVisibility = SeparatorVisibility.Default,
            View header = null, 
            View footer = null,
            Thickness margin = default): base(ListViewCachingStrategy.RecycleElement)
        {
            this.HasUnevenRows = true;
            if (dataTemplateType != null)
            {
                this.ItemTemplate = new DataTemplate(dataTemplateType);
            }
            
            this.IsPullToRefreshEnabled = isPullDownToRefreshEnabled;
            this.SeparatorVisibility = separatorVisibility;
            this.Header = header;
            this.Footer = footer;
            
            if (margin != default)
            {
                this.Margin = margin;
            }
        } 
    }
}