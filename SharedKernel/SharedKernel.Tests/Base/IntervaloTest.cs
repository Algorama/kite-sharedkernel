using System;
using SharedKernel.Domain.ValueObjects;
using NUnit.Framework;

namespace SharedKernel.Tests.Base
{
    [TestFixture]
    public class IntervaloTest
    {
        private Intervalo _intervalo;

        [SetUp]
        public void Setup()
        {
            // Preparação dos Testes
            var inicio = new DateTime(2015, 03, 01);
            var fim = new DateTime(2015, 03, 10);
            _intervalo = new Intervalo(inicio, fim);
        }

        [Test]
        public void Pode_Retornar_Duracao_Em_Dias()
        {
            // EXECUÇÃO
            var retorno = _intervalo.RetornaDuracaoEmDias();

            // TESTE
            Assert.AreEqual(retorno, 9);
        }

        [Test]
        public void Pode_Retornar_Duracao_Em_Horas()
        {
            // EXECUÇÃO
            var retorno = _intervalo.RetornaDuracaoEmHoras();

            // TESTE
            Assert.AreEqual(retorno, 216);
        }

        [Test]
        public void Invervalo_E_Vigente()
        {
            // PREPARAÇÃO
            var target      = new DateTime(2015, 03, 05);

            // EXECUÇÃO
            var retorno = _intervalo.IntervaloVigente(target);

            // TESTE
            Assert.IsTrue(retorno);
        }

        [Test]
        public void Invervalo_Nao_Vigente()
        {
            // PREPARAÇÃO

            // EXECUÇÃO
            var retorno = _intervalo.IntervaloVigente();

            // TESTE
            Assert.IsFalse(retorno);
        }
    }
}