using System;
using System.Data;
using SharedKernel.Repository.SqlServer;
using NUnit.Framework;

namespace SharedKernel.Tests.Base
{
    [TestFixture]
    public class LeituraSqlServerTest
    {
        [Test]
        public void Pode_Ler_Dados_SqlServer()
        {
            // Acerte a Connection String!! Está apontando para um SQLServer local, database master
            // Use essa Connection String para autenticação com usuário SA (ou outro usuário do SQLServer)
            //var cs = "Server=(local); initial catalog=master; user id=sa; password=MinhaSenha";

            var cs = "Server=(local); initial catalog=master; Trusted_Connection=True;";
            
            var repositorio = new SqlClientHelper(cs);

            var comando = "SELECT * From information_schema.tables";

            var dados = repositorio.GetDataTable(comando);
            foreach (DataRow row in dados.Rows)
            {
                var nome = row["TABLE_NAME"].ToString();
                Console.WriteLine(nome);
            }
        }
    }
}