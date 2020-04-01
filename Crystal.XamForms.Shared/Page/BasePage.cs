using Crystal.XamForms.Shared.Abstraction;
using Crystal.XamForms.Shared.Ui;
using MvvmCross;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using Xamarin.Forms.Xaml;

namespace Crystal.XamForms.Shared.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public abstract class BasePage<TViewModel> : MvxContentPage<TViewModel> where TViewModel : MvxViewModel
    {
        protected IViewStore ViewStore { get; }

        protected BasePage()
        {
            var themeProperty = Mvx.IoCProvider.Resolve<IThemeProperty>();
            this.BackgroundColor = themeProperty.BackgroundColor;
            ViewStore = new ViewStore(themeProperty);
            this.SetupNavBar();
            this.SetupContent();
        }

        protected virtual void SetupNavBar()
        {
            //***
            //*** Override this method to setup Navigation Bar
            //*** 
        }

        protected virtual void SetupContent()
        {
            //***
            //*** Override this method to Setup Page Content
            //*** 
        }
    }
}