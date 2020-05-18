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
        protected IViewStore ViewStore { get; }

        protected BasePage()
        {
            try
            {
                IThemeProperty themeProperty = Mvx.IoCProvider.Resolve<IThemeProperty>();
                this.BackgroundColor = themeProperty.BackgroundColor;
                this.ViewStore = (IViewStore) new Crystal.XamForms.Shared.Ui.ViewStore(themeProperty);
            }
            catch (MvxIoCResolveException)
            {
                this.ViewStore = (IViewStore) new Crystal.XamForms.Shared.Ui.ViewStore();
            }
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