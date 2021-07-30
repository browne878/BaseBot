using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ArkDiscordHelper.Database
{
    public class DBUtils
    {
        public static MySqlConnection GetDBConnection(string _mysqlhost, string _mysqldatabase, string _mysqlusername, string _mysqlpass, int _mysqlport )
        {
            string host = _mysqlhost;
            string database = _mysqldatabase;
            string username = _mysqlusername;
            string password = _mysqlpass;
            int port = _mysqlport;

            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }
    }
}
