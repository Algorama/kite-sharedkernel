using System;
using System.Collections.Generic;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Repositorio;

namespace Kite.Base.Repositorio.Mock
{
    public class MockRepositorioSessao : IRepositorioSessao
    {
        private static Dictionary<string, object> _repositorios;

        public IRepositorioConsulta<T> GetRepositorioConsulta<T>() where T : EntidadeBase
        {
            if (_repositorios == null)
                _repositorios = new Dictionary<string, object>();

            var entidadeNome = typeof(T).Name;

            if (!_repositorios.ContainsKey(entidadeNome))
                _repositorios.Add(entidadeNome, new MockRepositorioConsulta<T>());

            return _repositorios[entidadeNome] as MockRepositorioConsulta<T>;
        }

        public IRepositorio<T> GetRepositorio<T>() where T : EntidadeBase, IAggregateRoot
        {
            if (_repositorios == null)
                _repositorios = new Dictionary<string, object>();

            var entidadeNome = typeof(T).Name;

            if (!_repositorios.ContainsKey(entidadeNome))
                _repositorios.Add(entidadeNome, new MockRepositorio<T>());

            return _repositorios[entidadeNome] as MockRepositorio<T>;
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