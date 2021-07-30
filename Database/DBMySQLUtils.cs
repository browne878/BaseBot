using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace ArkDiscordHelper.Database
{
    class DBMySQLUtils
    {
        public static MySqlConnection
               GetDBConnection(string _host, int _port, string _database, string _username, string _password)
        {
            // Connection String.
            String connString = "Server=" + _host + ";Database=" + _database
                + ";port=" + _port + ";User Id=" + _username + ";password=" + _password;

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }
    }
}
