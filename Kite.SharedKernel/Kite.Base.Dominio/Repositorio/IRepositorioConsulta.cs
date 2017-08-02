using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kite.Base.Dominio.Entidades;

namespace Kite.Base.Dominio.Repositorio
{
    public interface IRepositorioConsulta<T> where T : EntidadeBase
    {
        T Retorna(long id);

        IQueryable<T> Consulta();

        IQueryable<T> Consulta(Expression<Func<T, bool>> where);

        IList<T> Consulta(string comandoHql);
    }
}