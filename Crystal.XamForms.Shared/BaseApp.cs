using System;
using MvvmCross.ViewModels;

namespace Crystal.XamForms.Shared
{
    public class BaseApp : MvxApplication
    {
        public override void Initialize()
        {
                this.OnStart();
                this.Registrations();
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
