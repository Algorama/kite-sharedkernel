using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace SharedKernel.Repository.NHibernate
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory()
        {
            if (_sessionFactory != null) return _sessionFactory;

            var config = new Configuration().Configure(GetPath() 
                + "\\nhibernate.cfg.xml");
            _sessionFactory = config.BuildSessionFactory();
            return _sessionFactory;
        }

        public static string GetPath()
        {
            var x = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var path = x.LocalPath.ToLower().Replace("\\sharedkernel.repository.dll", "");
            return path;
        }

        public static ISession OpenSession()
        {
            return SessionFactory().OpenSession();
        }

        public static void CreateDb()
        {
            var config = new Configuration().Configure(GetPath() + "\\nhibernate.cfg.xml");
            var schemaExport = new SchemaExport(config);

            // Mudar o segundo parametro para FALSE para não
            // apagar o banco de dados
            //schemaExport.Drop(true, true);
            schemaExport.Create(true, true);
        }
    }
}