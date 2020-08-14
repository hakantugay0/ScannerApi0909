using Autofac;
using Scanner.Core.Abstract;
using Scanner.Data.Concrete.Repositories;
using Scanner.Data.Concrete.UnitOfWork;
using Scanner.Helper.Security.JWT;
using Scanner.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Resolver.DependencyResolver
{
    public class AutofacResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerDependency();

            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
        }
    }
}
