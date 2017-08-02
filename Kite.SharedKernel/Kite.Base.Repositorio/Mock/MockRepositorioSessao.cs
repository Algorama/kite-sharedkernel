using System;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Repositorio;

namespace Kite.Base.Repositorio.Mock
{
    public class MockRepositorioSessao : IRepositorioSessao
    {
        public IRepositorioConsulta<T> GetRepositorioConsulta<T>() where T : EntidadeBase
        {
            return new MockRepositorioConsulta<T>();
        }

        public IRepositorio<T> GetRepositorio<T>() where T : EntidadeBase, IAggregateRoot
        {
            return new MockRepositorio<T>();
        }

        public void IniciaTransacao()
        {
            Console.WriteLine("Mock: Transação Iniciada");
        }

        public void ComitaTransacao()
        {
            Console.WriteLine("Mock: Transação Commitada");
        }

        public void RollBackTransacao()
        {
            Console.WriteLine("Mock: RollBack da Transação");
        }

        public void Dispose()
        {
            Console.WriteLine("Mock: Dispose da Sessão");
        }
    }
}