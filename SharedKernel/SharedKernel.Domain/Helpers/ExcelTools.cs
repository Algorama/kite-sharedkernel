using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace SharedKernel.Domain.Helpers
{
    public class ExcelTools
    {
        // Instalar o Provider de acesso 32 ou 64, de acordo com a versão do Office
        // Se o Office for 32 e o seu computador for 64, mude o projeto de Teste para 32
        // https://www.microsoft.com/en-us/download/details.aspx?id=13255
        public static DataTable XlsxToDataTable(string file, string sheet)
        {
            if (!File.Exists(file))
                throw new Exception("Atenção, o arquivo selecionado não foi encontrado");

            var connSt = "Provider=Microsoft.ACE.OLEDB.12.0;DataExpiracao Source=" + file.Trim() + ";" + "Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";

            DataTable dt;

            using (var conn = new OleDbConnection(connSt))
            {
                conn.Open();
                var oda = new OleDbDataAdapter($"SELECT * FROM [{sheet}$]", conn);
                var ds = new DataSet();
                oda.Fill(ds);
                dt = ds.Tables[0];
            }

            return dt;
        }
    }
}
