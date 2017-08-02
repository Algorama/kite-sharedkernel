using Ninject;
using Kite.Base.Dominio.Repositorio;
using Kite.Base.Repositorio;
using Kite.Base.Repositorio.Mock;

namespace Kite.Base.Util
{
    public class Kernel
    {
        private static StandardKernel _kernel;

        public static void Start()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IRepositorioHelper>().To<RepositorioHelper>();
        }

        public static void StartMock()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IRepositorioHelper>().To<MockRepositorioHelper>();
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
