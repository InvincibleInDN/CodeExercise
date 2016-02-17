using CodeExecise.Services.Implementations;
using CodeExecise.Services.Interfaces;
using Ninject.Modules;

namespace CodeExecise.Services
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPaymentService>().To<PaymentService>();
        }
    }
}
