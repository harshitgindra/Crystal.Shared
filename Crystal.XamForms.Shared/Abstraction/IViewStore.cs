using System;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Abstraction
{
    public interface IViewStore
    {
        IThemeProperty ThemeProperty { get; set; }

        Button Button(string text = default,
            Thickness padding = default,
            Thickness margin = default);

        Frame Frame(View content = default,
            Thickness padding = default,
            Thickness margin = default);

        Image Image(double widthRequest = default,
            ImageSource source = default);

        Label Label(string text = null,
            NamedSize fontSize = NamedSize.Default,
            TextAlignment horizontalTextAlignment = TextAlignment.Start,
            Thickness padding = default,
            Thickness margin = default,
            FontAttributes fontAttributes = default,
            TextAlignment verticalTextAlignment = TextAlignment.Start);

        ListView ListView(Type dataTemplateType = default,
            bool isPullDownToRefreshEnabled = false,
            SeparatorVisibility separatorVisibility = SeparatorVisibility.Default,
            View header = null,
            View footer = null,
            Thickness margin = default);

        StackLayout StackLayout(Thickness padding = default,
            Thickness margin = default,
            View[] children = default);

        Editor Editor(string text = default,
            double fontSize = default,
            string placeHolder = default,
            Thickness margin = default
        );

        ToolbarItem ToolbarItem(ImageSource imageSource = default,
            int priority = default,
            string text = default,
            Command command = default);

        ImageButton ImageButton(Thickness margin = default,
            ImageSource imageSource = default,
            Command command = default);
    }
}