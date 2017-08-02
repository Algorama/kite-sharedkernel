using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Repositorio;

namespace Kite.Base.Repositorio
{
    public class RepositorioConsulta<T> : IRepositorioConsulta<T> where T : EntidadeBase
    {
        protected ISession Sessao { get; set; }

        public RepositorioConsulta(ISession sessao)
        {
            Sessao = sessao;
        }

        public T Retorna(long id)
        {
            return Sessao.Get<T>(id);
        }

        public IQueryable<T> Consulta()
        {
            return Sessao.Query<T>();
        }

        public IQueryable<T> Consulta(Expression<Func<T, bool>> @where)
        {
            return Sessao.Query<T>().Where(@where);
        }

        public IList<T> Consulta(string comandoHql)
        {
            //var query = Sessao.CreateQuery(comandoHql);
            //var listaInterna = query.List<T>();
            //var listaRetorno = listaInterna.ToList();
            //return listaRetorno;

            return Sessao.CreateQuery(comandoHql).List<T>().ToList();
        }
    }
}