using ArkDiscordHelper.Models;
using ArkDiscordHelper.Services;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArkDiscordHelper.Services
{
    public class ArkCommandManager
    {
        private readonly DiscordClient Bot;
        private readonly Config Config;
        private readonly Config ConfigCommands;
        private readonly FileService FileManager;
        private readonly RconManager RconManager;
         
        public ArkCommandManager(DiscordClient bot, RconManager rconManager, Config config, FileService fileManager)
        {
            Bot = bot;
            RconManager = rconManager;
            Config = config;
            FileManager = fileManager;
            Config = fileManager.GetConfig();
        }
       /*
        public async Task InitializeAsync()
        {

    

            Bot.MessageCreated += async (client, args) =>
            {
                _ = Task.Run(() => MessageAdded(args));

                await Task.CompletedTask;
            };

        }

        public async Task MessageAdded(MessageCreateEventArgs args)
        {
            if (args.Channel.Id == 614901166232305696)
            {
                Console.WriteLine(args.Message.Content);
            }
        }
                /*
                 * 
            if ((args.Channel.Id == Config.DiscordOptions.ChannelRequestPVPVE) || (args.Channel.Id == Config.DiscordOptions.ChannelRequestPVP))
            {

                if (args.Channel.Id == Config.DiscordOptions.ChannelRequestPVPVE)
                {
                    CluserName = "PVPVE";
                }
                if (args.Channel.Id == Config.DiscordOptions.ChannelRequestPVP)
                {
                    CluserName = "PVP";
                }
                Console.Writeline(args.Message.Embeds);
                var content = args.Message.Embeds[0].Description.ToString();
                int i = 0;
                sb.Append(content).Replace('\n', '|');
                var newinfostring = sb.ToString();

                while (newinfostring[i] != ':')
                {
                    i++;
                    if (newinfostring[i].Equals(':'))
                    {

                        i += 2;
                        while (newinfostring[i] != '|')
                        {
                            IngamePlayerName += content[i++];
                        }

                        break;
                    }
                }
                while (newinfostring[i] != ':')
                {
                    i++;

                    if (newinfostring[i].Equals(':'))
                    {
                        i += 2;
                        while (newinfostring[i] != '|')
                        {
                            TribeName += content[i++];
                        }

                        break;
                    }
                }
                while (newinfostring[i] != ':')
                {
                    i++;
                    if (newinfostring[i].Equals(':'))
                    {
                        i += 2;
                        while (newinfostring[i] != '|')
                        {
                            SteamID += content[i++];
                        }
                        break;

                    }
                }
                while (newinfostring[i] != ':')
                {
                    i++;
                    if (newinfostring[i].Equals(':'))
                    {
                        i += 2;
                        while (newinfostring[i] != '|')
                        {
                            TribeID += content[i++];
                        }
                        break;
                    }
                }
                while (newinfostring[i] != ':')
                {
                    i++;
                    if (newinfostring[i].Equals(':'))
                    {
                        i += 2;
                        while (newinfostring[i] != '|')
                        {
                            IngameKitName += content[i++];
                        }
                        break;
                    }
                }
                while (newinfostring[i] != ':')
                {
                    i++;
                    if (newinfostring[i].Equals(':'))
                    {
                        i += 2;

                        while (newinfostring[i] != '|')
                        {
                            MapName += content[i++];

                        }
                        if (MapName.EndsWith('P'))
                        {
                            MapName = MapName.Remove(MapName.Length - 2);
                        }
                        break;
                    }
                }
                 * 
                 * 
                 * 
            }
           
        }*/
        public async Task<string> RconSendCommand(string command, string servername)
        {
            int serverid = -1;
            for (int i = 0; i < Config.Servers.Count; i++)
            {
                if (Config.Servers[i].ServerName == servername)
                {
                    serverid = i;
                }
            }
            var result = await RconManager.RconCommand(command, serverid);
            return result;
        }
    }
}
