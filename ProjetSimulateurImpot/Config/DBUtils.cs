using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSimulateurImpot.Config
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "127.0.0.1", database = "projet_dotnet_simulateur_impot", username = "phpmyadmin", password = "phpmyadmin";
            int port = 3306;
            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }
        public static string GetDBConnectionString()
        {
            string host = "127.0.0.1", database = "projet_dotnet_simulateur_impot", username = "phpmyadmin", password = "phpmyadmin";
            int port = 3306;
            return DBMySQLUtils.GetDBConnectionString(host, port, database, username, password);
        }
    }
}
