using System;
using Crystal.XamForms.Shared.Abstraction;
using MvvmCross;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class ViewStore : IViewStore
    {
        public ViewStore(IThemeProperty themeProperty) : this()
        {
            ThemeProperty = themeProperty;
        }

        public ViewStore()
        {
            Mvx.IoCProvider.TryResolve(out IThemeProperty themeProperty);
            ThemeProperty = themeProperty;
        }

        public IThemeProperty ThemeProperty { get; set; }

        public Button Button(string text = default,
            Thickness padding = default,
            Thickness margin = default)
        {
            return new BaseButton(ThemeProperty, text, padding, margin);
        }

        public Frame Frame(View content = default,
            Thickness padding = default,
            Thickness margin = default)
        {
            return new BaseFrame(ThemeProperty, content, padding, margin);
        }

        public Image Image(double widthRequest = default,
            ImageSource source = default)
        {
            return new BaseImage(ThemeProperty, widthRequest, source);
        }

        public Label Label(string text = null,
            NamedSize fontSize = NamedSize.Default,
            TextAlignment horizontalTextAlignment = TextAlignment.Start,
            Thickness padding = default,
            Thickness margin = default,
            FontAttributes fontAttributes = default,
            TextAlignment verticalTextAlignment = TextAlignment.Start)
        {
            return new BaseLabel(ThemeProperty, text, fontSize, horizontalTextAlignment, padding, margin,
                fontAttributes, verticalTextAlignment);
        }

        public ListView ListView(Type dataTemplateType = default,
            bool isPullDownToRefreshEnabled = false,
            SeparatorVisibility separatorVisibility = SeparatorVisibility.Default,
            View header = null,
            View footer = null,
            Thickness margin = default)
        {
            return new BaseListView(ThemeProperty, dataTemplateType, isPullDownToRefreshEnabled,
                separatorVisibility, header, footer, margin);
        }

        public StackLayout StackLayout(Thickness padding = default,
            Thickness margin = default,
            View[] children = default)
        {
            return new BaseStackLayout(ThemeProperty, padding, margin, children);
        }

        public Editor Editor(string text = default,
            double fontSize = default,
            string placeHolder = default,
            Thickness margin = default
        )
        {
            return new BaseEditor(ThemeProperty, text, fontSize, placeHolder, margin);
        }

        public ToolbarItem ToolbarItem(ImageSource imageSource = default,
            int priority = default,
            string text = default,
            Command command = default)
        {
            return new BaseToolbarItem(ThemeProperty, imageSource, priority, text, command);
        }

        public ImageButton ImageButton(Thickness margin = default,
            ImageSource imageSource = default,
            Command command = default)
        {
            return new BaseImageButton(ThemeProperty, margin, imageSource, command);
        }
    }
}