using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Facturacion.data.Utilities
{
    public class DataHelper
    {
        private static DataHelper? _instance;
        private SqlConnection _connection;
        public DataHelper()
        {
            _connection = new SqlConnection(Properties.Resources.connection);
        }
        public SqlConnection GetConnection()
        {
            return _connection;
        }
        public static DataHelper GetInstance() 
        {
            if (_instance == null)
            {
                _instance = new DataHelper();
            }
            return _instance;
        }
        public DataTable ? ExecuteSPquery(string sp, List<ParameterSP>? parameter = null)
        {
            DataTable ? dt = new DataTable();
            try
            {
                _connection.Open();
                var cmd = new SqlCommand(sp, _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp;
                if (parameter != null)
                {
                    foreach (var param in parameter)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }
                }
                dt.Load(cmd.ExecuteReader());
            }
            catch
            {
                dt = null;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return dt;
        }
    }
}
