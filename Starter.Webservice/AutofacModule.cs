using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Starter.Webservice
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new Assembly[]
            {
                Assembly.GetAssembly(typeof(Business.Anchor)),
                Assembly.GetAssembly(typeof(Services.Anchor))
            };

            builder.RegisterAssemblyTypes(assemblies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.IsSubclassOf(typeof(FluentValidation.IValidator)))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }
    }
}
