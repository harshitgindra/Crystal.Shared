using Crystal.XamForms.Shared.Abstraction;
using Crystal.XamForms.Shared.Ui;
using MvvmCross;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Page
{
    public class BaseViewCell : ViewCell
    {
        public BaseViewCell()
        {
            var themeProperty = Mvx.IoCProvider.Resolve<IThemeProperty>();
            ViewStore = new ViewStore(themeProperty);
        }

        protected IViewStore ViewStore { get; set; }
    }
}