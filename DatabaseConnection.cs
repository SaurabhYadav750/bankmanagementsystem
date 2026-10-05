using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace bankmanagementsystem
{
     class DatabaseConnection
    {
        //string connectionString
            string connectionString =
              "Data Source=LAPTOP-M2NIRARV;Initial Catalog=BankDB;Integrated Security=True";

        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            
            return connection;
        }
    }
}
