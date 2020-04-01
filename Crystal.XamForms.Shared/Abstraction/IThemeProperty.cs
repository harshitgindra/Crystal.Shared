using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Abstraction
{
    public interface IThemeProperty
    {
        Color TextColor { get; set; }
        Color BackgroundColor { get; set; }
        string FontFamily { get; set; }
    }
}