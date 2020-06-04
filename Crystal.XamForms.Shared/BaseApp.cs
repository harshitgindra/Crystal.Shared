using MvvmCross.ViewModels;

namespace Crystal.XamForms.Shared
{
    public class BaseApp : MvxApplication
    {
        public override void Initialize()
        {
            OnStart();
            Registrations();
            base.Initialize();
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void Registrations()
        {
        }
    }
}