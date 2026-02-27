using Autofac;
using MyApp.Core.Interfaces;
using MyApp.Models;
using MyApp.Repositories;
using MyApp.Infrastructure;

namespace MyApp.Web.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>))
                   .As(typeof(IRepository<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
                   .As<IUserRepository>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<AppDbContext>()
                   .AsSelf()
                   .InstancePerLifetimeScope();
        }
    }
}
