using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Repositorio;

namespace Kite.Base.Repositorio.Mock
{
    public class MockRepositorioConsulta<T> : IRepositorioConsulta<T> where T : EntidadeBase
    {
        public static List<T> Data { get; set; }

        public MockRepositorioConsulta()
        {
            Data = new List<T>();
        }

        public T Retorna(long id)
        {
            return Data.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<T> Consulta()
        {
            return Data.AsQueryable();
        }

        public IQueryable<T> Consulta(Expression<Func<T, bool>> @where)
        {
            return Data.AsQueryable().Where(where);
        }

        public IList<T> Consulta(string comandoHql)
        {
            return Data;
        }
    }
}