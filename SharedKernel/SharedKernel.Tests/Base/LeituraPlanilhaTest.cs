using System;
using System.Data;
using SharedKernel.Domain.Helpers;
using NUnit.Framework;

namespace SharedKernel.Tests.Base
{
    [TestFixture]
    public class LeituraPlanilhaTest
    {
        [Test]
        public void Pode_Ler_Dados_Planilha()
        {
            // Acerte o caminho do arquivo aqui!!
            var arquivo = @"C:\Fontes\Algorama\github\kite-sharedkernel\SharedKernel\SharedKernel.Tests\Files\planilha.xlsx";

            var dados = ExcelTools.XlsxToDataTable(arquivo, "clientes");
            foreach (DataRow row in dados.Rows)
            {
                Console.WriteLine($"{row["Codigo"]} - {row["Nome"]}");
            }
            Assert.Greater(dados.Rows.Count, 0);
        }
    }
}