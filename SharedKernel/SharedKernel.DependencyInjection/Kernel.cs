using Ninject;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Services;
using SharedKernel.Repository.Mock;
using SharedKernel.Repository.NHibernate;

namespace SharedKernel.DependencyInjection
{
    public class Kernel
    {
        private static StandardKernel _kernel;

        private static void StartBase()
        {
            _kernel = new StandardKernel();
            _kernel.Bind(typeof(IQueryService<>)).To(typeof(QueryService<>));
            _kernel.Bind(typeof(ICrudService<>)).To(typeof(CrudService<>));
        }

        public static void Start()
        {
            StartBase();
            _kernel.Bind<IHelperRepository>().To<HelperRepository>();
        }

        public static void StartMock()
        {
            StartBase();
            _kernel.Bind<IHelperRepository>().To<MockHelperRepository>();
        }

        public static T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public static void Bind<TFrom, TTo>() where TTo : TFrom
        {
            _kernel.Bind<TFrom>().To<TTo>();
        }
    }
}