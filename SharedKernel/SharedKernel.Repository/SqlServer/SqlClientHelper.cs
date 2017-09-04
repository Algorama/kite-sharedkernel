using System.Data;
using System.Data.SqlClient;

namespace SharedKernel.Repository.SqlServer
{
    public class SqlClientHelper
    {
        public string ConnectionString{ get; set;}

        public SqlClientHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DataTable GetDataTable(string commandText)
        {
            var conn = new SqlConnection(ConnectionString);
            var dt = new DataTable();
            try
            {
                conn.Open();
                var comm = new SqlCommand
                {
                    CommandText = commandText,
                    CommandType = CommandType.Text,
                    Connection = conn,
                    CommandTimeout = 120
                };

                var da = new SqlDataAdapter { SelectCommand = comm };
                da.Fill(dt);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public DataTable GetDataTable(string tabela, string where, string order = null)
        {
            var conn = new SqlConnection(ConnectionString);
            var dt = new DataTable();
            try
            {
                var commandText = string.IsNullOrWhiteSpace(where) ? $"SELECT * FROM {tabela}" : $"SELECT * FROM {tabela} WHERE {where}";

                if (!string.IsNullOrWhiteSpace(order))
                    commandText += $" ORDER BY {order}";

                conn.Open();
                var comm = new SqlCommand
                {
                    CommandText = commandText,
                    CommandType = CommandType.Text,
                    Connection = conn,
                    CommandTimeout = 120
                };

                var da = new SqlDataAdapter { SelectCommand = comm };
                da.Fill(dt);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public DataTable GetDataTable(string tabela, int inicio, int limite, string where = null, string order = null)
        {
            var conn = new SqlConnection(ConnectionString);
            var dt = new DataTable();
            try
            {
                if(string.IsNullOrWhiteSpace(order)) order = "Id";

                var commandText =
                    string.Format(
                        "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY {1} ) AS Indice, * FROM {0}) AS RowConstrainedResult {2}",
                        tabela,
                        order,
                        "WHERE Indice BETWEEN 1 AND 1000 ORDER BY Indice");
                    
                conn.Open();
                var comm = new SqlCommand
                {
                    CommandText = commandText,
                    CommandType = CommandType.Text,
                    Connection = conn,
                    CommandTimeout = 120
                };

                var da = new SqlDataAdapter { SelectCommand = comm };
                da.Fill(dt);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public DataTable GetDataReader(string tabela, string where = null, string order = null)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                var commandText = string.IsNullOrWhiteSpace(where) ? $"SELECT * FROM {tabela}" : $"SELECT * FROM {tabela} WHERE {where}";

                if (!string.IsNullOrWhiteSpace(order))
                    commandText += $" ORDER BY {order}";

                var cmd = new SqlCommand(commandText, conn);
                conn.Open();
                var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                var dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            finally
            {
                conn?.Close();
            }
        }

        public DataTable GetDataTableWithQuery(string query)
        {
            var conn = new SqlConnection(ConnectionString);
            var dt = new DataTable();
            try
            {
                conn.Open();
                var comm = new SqlCommand
                {
                    CommandText = query,
                    CommandType = CommandType.Text,
                    Connection = conn
                };

                var da = new SqlDataAdapter { SelectCommand = comm };
                da.Fill(dt);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public DataRowView GetDataRowView(string tabela, long id)
        {
            var dt = GetDataTable(tabela, $"Id = {id}");
            return dt.Rows.Count > 0 ? dt.DefaultView[0] : null;
        }

        public bool RunCommand(string comando)
        {
            var conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                var comm = new SqlCommand
                {
                    CommandText = comando,
                    CommandType = CommandType.Text,
                    Connection = conn
                };

                comm.ExecuteScalar();
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        public bool RunCommand(string[] comandos)
        {
            var conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                foreach (var comando in comandos)
                {
                    var comm = new SqlCommand
                    {
                        CommandText = comando,
                        CommandType = CommandType.Text,
                        Connection = conn
                    };
                    comm.ExecuteScalar();    
                }
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        public decimal RunSum(string comando)
        {
            decimal resultado;
            var conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                var comm = new SqlCommand
                {
                    CommandText = comando,
                    CommandType = CommandType.Text,
                    Connection = conn
                };

                resultado = comm.ExecuteScalar() is decimal ? (decimal) comm.ExecuteScalar() : 0;
            }
            finally
            {
                conn.Close();
            }
            return resultado;
        }

        public bool TestConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
            }
            finally
            {
                conn.Close();
            }
            return true;
        }
    }
}
