using DSharpPlus;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ArkDiscordHelper.Models;
using ArkDiscordHelper.Services;
using ArkDiscordHelper.Database;

namespace ArkDiscordHelper.Services
{
    public class DatabaseManager
    {
        private readonly DiscordClient Bot;
        private readonly Config Config;
        private readonly FileService FileManager;
        private readonly RconManager RconManager;

        public DatabaseManager(DiscordClient _bot, RconManager _rconManager, Config _config, FileService _fileManager)
        {
            Bot = _bot;
            RconManager = _rconManager;
            Config = _config;
            FileManager = _fileManager;
            Config = _fileManager.GetConfig();
        }

        public MySqlConnection MysqlConnection(int _dbindex)
        {

            var conn = DBUtils.GetDBConnection(Config.MySql.MySqlHost, Config.MySql.MySqlDB, Config.MySql.MySqlUser, Config.MySql.MySqlPass, Config.MySql.MySqlPort);
            conn.Open();
            return conn;
            // stops here

        }

        public string GetSteamId(ulong _discordID)
        {
            string sql_result = "0";
            string sql = "SELECT SteamID FROM discord_vote_rewards WHERE DiscordID = " + _discordID;
            // Create command.
            MySqlCommand cmd = new MySqlCommand();

            // Set connection for command.
            cmd.Connection = MysqlConnection(0);
            cmd.CommandText = sql;


            try
            {
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            sql_result = reader.GetString(0);
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                // Close connection.
                MysqlConnection(0).Close();
                // Dispose object, Freeing Resources.
                MysqlConnection(0).Dispose();
            }

            return sql_result;

        }

        public string GetDiscordId(ulong _steamID)
        {
            string sql_result = "0";
            string sql = "SELECT DiscordID FROM discord_vote_rewards WHERE SteamID =" + _steamID;
            // Create command.
            MySqlCommand cmd = new MySqlCommand();

            // Set connection for command.
            cmd.Connection = MysqlConnection(0);
            cmd.CommandText = sql;


            try
            {
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            sql_result = reader.GetString(0);
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                // Close connection.
                MysqlConnection(0).Close();
                // Dispose object, Freeing Resources.
                MysqlConnection(0).Dispose();
            }

            return sql_result;

        }
    }
}


