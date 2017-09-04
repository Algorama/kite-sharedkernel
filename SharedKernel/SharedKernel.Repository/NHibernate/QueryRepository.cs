using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharedKernel.Domain.Dtos;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using NHibernate;
using NHibernate.Linq;
using NHibernate.OData;

namespace SharedKernel.Repository.NHibernate
{
    public class QueryRepository<T> : IQueryRepository<T> where T : EntityBase
    {
        protected ISession Session { get; set; }

        public QueryRepository(ISession session)
        {
            Session = session;
        }

        public T Get(long id)
        {
            return Session.Get<T>(id);
        }

        public IQueryable<T> GetAll()
        {
            return Session.Query<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> @where)
        {
            return Session.Query<T>().Where(@where);
        }

        public IList<T> GetByHql(string comandoHql)
        {
            return Session.CreateQuery(comandoHql).List<T>().ToList();
        }

        public IList<T> GetBySql(string sql)
        {
            var result = Session.CreateSQLQuery(sql).AddEntity(typeof(T));
            var res = result.List<T>().ToList();
            return res;
        }

        public ODataResult<T> GetOData(List<KeyValuePair<string, string>> queryStringParts)
        {
            //Separo e removo o $inlinecount pq o ODataQuery não implementa 
            var inlinecount = queryStringParts.FirstOrDefault(x => x.Key == "$inlinecount");
            queryStringParts.Remove(inlinecount);

            //Realiza a consulta
            var dados = Session.ODataQuery<T>(queryStringParts, new ODataParserConfiguration { CaseSensitiveLike = false }).List<T>();

            //Verifica se vai ter Count
            var count = inlinecount.Value == "allpages";

            if (!count) return new ODataResult<T>(dados);

            //Adiciona o clausula $count e executa a query
            queryStringParts.Add(new KeyValuePair<string, string>("$count", "true"));

            //remove a clausula orderby para realizar o count
            var orderby = queryStringParts.FirstOrDefault(x => x.Key == "$orderby");
            queryStringParts.Remove(orderby);
            var result = Session.ODataQuery<T>(queryStringParts, new ODataParserConfiguration { CaseSensitiveLike = false }).List();

            return new ODataResult<T>(result.Count > 0 ? (int)result[0] : 0, dados);
        }
    }
}