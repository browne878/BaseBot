using ArkDiscordHelper.Models;
using ArkDiscordHelper.Models.AlphaEngrams;
using DSharpPlus;
using RconSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArkDiscordHelper.Services
{
   public class RconManager
    {
        private readonly DiscordClient Bot;
        private readonly Config Config;
        private readonly RoadToAlphas RoadToAlphas;
        private readonly FileService FileManager;
        private RconClient RconClient;

        public RconManager(DiscordClient bot, Config config, FileService fileManager, RoadToAlphas roadToAlphas)
        {
            this.Bot = bot;
            this.Config = config;
            this.RoadToAlphas = roadToAlphas;
            this.FileManager = fileManager;
            this.Config = FileManager.GetConfig();
        }

        public async Task<Boolean> OpenRcon(int serverid)
        {

            RconClient = RconClient.Create(Config.Servers[serverid].RconIP, Config.Servers[serverid].RconPort);
            await RconClient.ConnectAsync();
            var isAuth = await RconClient.AuthenticateAsync(Config.Servers[serverid].RconPass);
            return isAuth;
        }


        public async Task<string> RconCommand(string command, int serverid)
        {
            //serverid = check which server in array for execute
            //openRcon check if connection is valid
            //command like addpoints etc
            if (await OpenRcon(serverid) == true)
            {
                var response = await RconClient.ExecuteCommandAsync(command);
                RconClient.Disconnect();

                //response is rcon respond like if you use !rcon island etc
                return response;

            }
            else
            {
                RconClient.Disconnect();
                return "Server is offline.";

            }

        }

    }
}
