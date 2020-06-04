using Crystal.XamForms.Shared.Abstraction;
using Crystal.XamForms.Shared.Ui;
using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using Xamarin.Forms.Xaml;

namespace Crystal.XamForms.Shared.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public abstract class BasePage<TViewModel> : MvxContentPage<TViewModel> where TViewModel : MvxViewModel
    {
        protected BasePage()
        {
            try
            {
                var themeProperty = Mvx.IoCProvider.Resolve<IThemeProperty>();
                BackgroundColor = themeProperty.BackgroundColor;
                ViewStore = new ViewStore(themeProperty);
            }
            catch (MvxIoCResolveException)
            {
                ViewStore = new ViewStore();
            }

            SetupNavBar();
            SetupContent();
        }

        protected IViewStore ViewStore { get; }

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