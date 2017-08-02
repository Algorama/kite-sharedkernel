using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Repositorio;
using Kite.Base.Dominio.ViewModels;

namespace Kite.Base.Dominio.Servicos
{
    public class ServicoConsulta<T> where T : EntidadeBase
    {
        private const int PageSize = 15;

        public List<string> Mensagens { get; set; }

        public IRepositorioHelper Helper { get; }
        
        public ServicoConsulta(IRepositorioHelper helper)
        {
            Helper = helper;
            Mensagens = new List<string>();
        }

        public virtual T Retorna(long id)
        {
            Mensagens.Clear();
            using (var sessao = Helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorioConsulta<T>();
                try
                {
                    sessao.IniciaTransacao();
                    var entidade = repo.Retorna(id);
                    sessao.ComitaTransacao();
                    return entidade;                    
                }
                catch (Exception ex)
                {
                    sessao.RollBackTransacao();
                    Mensagens.Add(ex.InnerException?.Message ?? ex.Message);
                    return default(T);
                }                
            }
        }

        public virtual IList<T> Consulta()
        {
            Mensagens.Clear();
            using (var sessao = Helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorioConsulta<T>();
                try
                {
                    sessao.IniciaTransacao();
                    var entidades = repo.Consulta().ToList();
                    sessao.ComitaTransacao();
                    return entidades;
                }
                catch (Exception ex)
                {
                    sessao.RollBackTransacao();
                    Mensagens.Add(ex.InnerException?.Message ?? ex.Message);
                    throw;
                }
            }
        }

        public virtual IList<T> Consulta(Expression<Func<T, bool>> @where)
        {
            Mensagens.Clear();
            using (var sessao = Helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorioConsulta<T>();
                try
                {
                    sessao.IniciaTransacao();
                    var entidades = repo.Consulta(@where).ToList();
                    sessao.ComitaTransacao();
                    return entidades;
                }
                catch (Exception ex)
                {
                    sessao.RollBackTransacao();
                    Mensagens.Add(ex.InnerException?.Message ?? ex.Message);
                    throw;
                }
            }
        }

        public virtual PageResult<T> ConsultaPaginada(int page)
        {
            Mensagens.Clear();
            using (var sessao = Helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorioConsulta<T>();
                try
                {
                    sessao.IniciaTransacao();

                    var result = new PageResult<T> { PaginaAtual = page };
                    var entidades = repo.Consulta();

                    result.TotalRegistros = entidades.Count();
                    result.TotalPaginas =
                        (int)Math.Ceiling((double)result.TotalRegistros / PageSize);

                    result.Resultado = entidades
                        .Skip(PageSize * page)
                        .Take(PageSize)
                        .ToList();

                    sessao.ComitaTransacao();
                    return result;
                }
                catch (Exception ex)
                {
                    sessao.RollBackTransacao();
                    Mensagens.Add(ex.InnerException?.Message ?? ex.Message);
                    throw;
                }
            }
        }
    }
}

