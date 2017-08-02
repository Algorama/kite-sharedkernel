using System;
using Kite.Base.Dominio.Entidades;

namespace Kite.Base.Dominio.Repositorio
{
    public interface IRepositorioSessao : IDisposable
    {
        IRepositorioConsulta<T> GetRepositorioConsulta<T>() where T : EntidadeBase;
        IRepositorio<T> GetRepositorio<T>() where T : EntidadeBase, IAggregateRoot;

        void IniciaTransacao();
        void ComitaTransacao();
        void RollBackTransacao();
    }
}