using ArkDiscordHelper.Models;
using ArkDiscordHelper.Models.AlphaEngrams;
using ArkDiscordHelper.Services;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkDiscordHelper.Commands
{
    using System.Text.RegularExpressions;

    public class RconCommands : BaseCommandModule
    {
        private readonly DiscordClient Bot;
        private readonly Config Config;
        private readonly DatabaseManager DatabaseManager;
        private readonly RoadToAlphas RoadToAlphas;
        private readonly FileService FileManager;
        private readonly ArkCommandManager ACM;
        private readonly ComBlocks Coms;
        public RconCommands(DiscordClient bot, Config config, FileService fileManager, ArkCommandManager acm, DatabaseManager databaseManager, RoadToAlphas roadToAlphas)
        {
            Bot = bot;
            Config = config;
            RoadToAlphas = roadToAlphas;
            FileManager = fileManager;
            DatabaseManager = databaseManager;
            ACM = acm;
            Config = fileManager.GetConfig();
            Coms = fileManager.GetComBlocks();
        }

        //Rcon Communication
        [Command("rcon")]
        [Description("Sends Rcon Command to PVE Ragnarok")]
        public async Task RconCommand(CommandContext ctx, string serverandclustername)
        {
            if (ctx.Channel.Id == Config.DiscordOptions.CommandsChannel)
            {
                List<string> messageContentList = ctx.Message.Content.Split(' ', '\n').ToList();
                var commands = new List<string>();
                var commandsCount = 0;

                foreach (string s in messageContentList)
                {
                    if (Regex.IsMatch(s, "^[/][rR][cC][oO][nN]$"))
                    {
                        commands.Add(s);
                        commandsCount++;
                    }
                    else
                    {
                        if (commands.Count > 0)
                        {
                            commands[commandsCount - 1] = commands[commandsCount - 1] + " " + s;
                        }
                        else
                        {
                            Console.WriteLine("error");
                        }
                    }
                }

                foreach (string temp in commands)
                {
                    var commandList = temp.Split(" ").ToList();

                    int mapClusterLength = commandList[1].Length;

                    string clusterPvpOrPve = commandList[1].Remove(0, (mapClusterLength - 3));
                    string clusterPvpve = "";

                    serverandclustername = commandList[1].ToLower();

                    string message = temp;

                    string rtaCommand = message.Substring((7 + mapClusterLength), 3);
                    string rtaLevel = message.Substring((7 + mapClusterLength), 4);

                    bool all = true;

                    if (serverandclustername.Equals("pvpve"))
                    {
                        for (int i = 0; i < Config.Servers.Count; i++)
                        {
                            if (Config.Servers[i].ClusterName.Equals("PVPVE"))
                            {
                                serverandclustername = Config.Servers[i].ServerName;
                                string command = message.Remove(0, (6 + mapClusterLength));
                                var respond = await ACM.RconSendCommand(command, serverandclustername);
                                await ctx.RespondAsync($"```{respond}```");
                                all = false;
                            }
                        }

                    }
                    else if (serverandclustername.Equals("pvp"))
                    {
                        for (int i = 0; i < Config.Servers.Count; i++)
                        {
                            if (Config.Servers[i].ClusterName.Equals("PVP"))
                            {
                                serverandclustername = Config.Servers[i].ServerName;
                                string command = message.Remove(0, (7 + mapClusterLength));
                                var respond = await ACM.RconSendCommand(command, serverandclustername);
                                await ctx.RespondAsync($"```{respond}```");
                                all = false;
                            }
                        }
                    }
                    else if (serverandclustername.Equals("pve"))
                    {
                        for (int i = 0; i < Config.Servers.Count; i++)
                        {
                            if (Config.Servers[i].ClusterName.Equals("PVE"))
                            {
                                serverandclustername = Config.Servers[i].ServerName;
                                string command = message.Remove(0, (6 + mapClusterLength));
                                var respond = await ACM.RconSendCommand(command, serverandclustername);
                                await ctx.RespondAsync($"```{respond}```");
                                all = false;
                            }
                        }
                    }
                    else
                    {
                        clusterPvpve = serverandclustername.Remove(0, (mapClusterLength - 5));
                    }

                    if (clusterPvpve.Equals("pvpve") && all)
                    {
                        string command = message.Remove(0, (6 + mapClusterLength));
                        //var respond = await ACM.RconSendCommand(command, serverandclustername);

                        bool rtaLoop = true;

                        var respond = "";

                        if (clusterPvpve.Equals("pvpve") && rtaCommand.Substring(0, 3).ToLower().Equals("rta"))
                        {
                            string rtaAscentionCommand = command.Substring(6);
                            command = message.Remove(0, (12 + mapClusterLength));
                            command = command.Insert(0, "UnlockEngram ");
                            command = command.Insert(command.Length, " ");
                            string referanceCommand = command;


                            for (int i = 0; i < RoadToAlphas.RoadToAlpha.Count; i++)
                            {
                                if (clusterPvpve.Equals(RoadToAlphas.RoadToAlpha[i].Cluster.ToLower()))
                                {
                                    if (rtaLevel.ToLower().Equals(RoadToAlphas.RoadToAlpha[i].Level.ToLower()))
                                    {
                                        for (int a = 0; a < RoadToAlphas.RoadToAlpha[i].Engrams.Count; a++)
                                        {
                                            command = command.Insert(command.Length, RoadToAlphas.RoadToAlpha[i].Engrams[a]);
                                            respond = await ACM.RconSendCommand(command, serverandclustername);
                                            await ctx.RespondAsync($"```{respond}```");
                                            command = referanceCommand;
                                        }
                                    }
                                }

                                rtaLoop = false;
                            }
                        }
                        else
                        {
                            respond = await ACM.RconSendCommand(command, serverandclustername);
                        }

                        if (respond.Length > 2000)
                        {
                            int numberMessage = 0;
                            int divideMessage = respond.Length;
                            int startPos = 0;
                            string trimedMessage;

                            while (divideMessage > 1900)
                            {
                                numberMessage++;
                                divideMessage = respond.Length / numberMessage;
                            }

                            while (numberMessage > 0)
                            {
                                if ((startPos + divideMessage) > respond.Length)
                                {
                                    divideMessage = respond.Length - startPos;
                                }

                                trimedMessage = respond.Substring(startPos, divideMessage);
                                startPos = startPos + divideMessage;
                                numberMessage--;
                                await ctx.RespondAsync($"```{trimedMessage}```");
                            }
                        }
                        else if (rtaLoop)
                        {
                            await ctx.RespondAsync($"```{respond}```");
                        }
                    }
                    else if ((clusterPvpOrPve.Equals("pve") || clusterPvpOrPve.Equals("pvp")) && all)
                    {
                        string command = message.Remove(0, (6 + mapClusterLength));
                        //var respond = await ACM.RconSendCommand(command, serverandclustername);

                        bool rtaLoop = true;

                        var respond = "";

                        if (clusterPvpOrPve.Equals("pvp") && rtaCommand.Substring(0, 3).ToLower().Equals("rta"))
                        {
                            command = message.Remove(0, (12 + mapClusterLength));
                            command = command.Insert(0, "UnlockEngram ");
                            command = command.Insert(command.Length, " ");
                            string referanceCommand = command;


                            for (int i = 0; i < RoadToAlphas.RoadToAlpha.Count; i++)
                            {
                                if (clusterPvpOrPve.Equals(RoadToAlphas.RoadToAlpha[i].Cluster.ToLower()))
                                {
                                    if (rtaLevel.ToLower().Equals(RoadToAlphas.RoadToAlpha[i].Level.ToLower()))
                                    {
                                        for (int a = 0; a < RoadToAlphas.RoadToAlpha[i].Engrams.Count; a++)
                                        {
                                            command = command.Insert(command.Length, RoadToAlphas.RoadToAlpha[i].Engrams[a]);
                                            respond = await ACM.RconSendCommand(command, serverandclustername);
                                            await ctx.RespondAsync($"```{respond}```");
                                            command = referanceCommand;
                                        }
                                    }
                                }

                                rtaLoop = false;
                            }
                        }
                        else
                        {
                            respond = await ACM.RconSendCommand(command, serverandclustername);
                        }

                        if (clusterPvpOrPve.Equals("pve") && rtaCommand.Substring(0, 3).ToLower().Equals("rta"))
                        {
                            command = message.Remove(0, (12 + mapClusterLength));
                            command = command.Insert(0, "UnlockEngram ");
                            command = command.Insert(command.Length, " ");
                            string referanceCommand = command;


                            for (int i = 0; i < RoadToAlphas.RoadToAlpha.Count; i++)
                            {
                                if (clusterPvpOrPve.Equals(RoadToAlphas.RoadToAlpha[i].Cluster.ToLower()))
                                {
                                    if (rtaLevel.ToLower().Equals(RoadToAlphas.RoadToAlpha[i].Level.ToLower()))
                                    {
                                        for (int a = 0; a < RoadToAlphas.RoadToAlpha[i].Engrams.Count; a++)
                                        {
                                            command = command.Insert(command.Length, RoadToAlphas.RoadToAlpha[i].Engrams[a]);
                                            respond = await ACM.RconSendCommand(command, serverandclustername);
                                            await ctx.RespondAsync($"```{respond}```");
                                            command = referanceCommand;
                                        }
                                    }
                                }

                                rtaLoop = false;
                            }
                        }
                        else
                        {
                            respond = await ACM.RconSendCommand(command, serverandclustername);
                        }

                        //respond = await ACM.RconSendCommand(command, serverandclustername);

                        if (respond.Length > 2000)
                        {
                            int numberMessage = 0;
                            int divideMessage = respond.Length;
                            int startPos = 0;
                            string trimedMessage;

                            while (divideMessage > 1900)
                            {
                                numberMessage++;
                                divideMessage = respond.Length / numberMessage;
                            }

                            while (numberMessage > 0)
                            {
                                if ((startPos + divideMessage) > respond.Length)
                                {
                                    divideMessage = respond.Length - startPos;
                                }

                                trimedMessage = respond.Substring(startPos, divideMessage);
                                startPos = startPos + divideMessage;
                                numberMessage--;
                                await ctx.RespondAsync($"```{trimedMessage}```");
                            }
                        }
                        else if (rtaLoop)
                        {
                            await ctx.RespondAsync($"```{respond}```");
                        }
                    }
                    else
                    {
                        if (all)
                        {
                            await ctx.Channel.SendMessageAsync("The Cluster was not entered correctly.");
                        }
                    }
                }
            }
        }

        //Lists Commands to communicate with Bloody Cluster
        [Command("rcon-Servers")]
        [Description("Shows commands to communicate with Bloody Cluster")]
        public async Task RconServers(CommandContext ctx)
        {
            if (ctx.Channel.Id == Config.DiscordOptions.CommandsChannel)
            {
                var embedReply = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Title = "Server Communication Commands"
                };
                embedReply.WithAuthor("Bloody Command Helper", null, "https://cdn.discordapp.com/attachments/752642174801281214/802311513913950268/large.5897a03b41dc0_ARKHumanIconCompressed.png.323620cc3ef77936cfbdf57eb869579f.png");
                embedReply.AddField("NOTE!:", "All Commands begin with /rcon", false);
                embedReply.AddField("Ragnarok", "RagnarokPve - RagnarokPvp - RagnarokPvpve", false);
                embedReply.AddField("Scorched Earth", "ScorchedPve - ScorchedPvp - ScorchedPvpve", false);
                embedReply.AddField("Extinction", "ExtinctionPve - ExtinctionPvp - ExtinctionPvpve", false);
                embedReply.AddField("Crystal Isles", "CrystalPve - CrystalPvp - CrystalPvpve", false);
                embedReply.AddField("Aberration", "AberrationPve - AberrationPvp - AberrationPvpve", false);
                embedReply.AddField("Genesis", "GenesisPve - GenesisPvp - GenesisPvpve", false);
                embedReply.AddField("Island", "IslandPve - IslandPvp - IslandPvpve", false);
                embedReply.AddField("Center", "CenterPve - CenterPvp - CenterPvpve", false);
                embedReply.AddField("Valguero", "ValgueroPve - ValgueroPvp - ValgueroPvpve", false);
                embedReply.WithFooter("These Commands are not Case Sensitive", null);

                await ctx.Channel.SendMessageAsync(embed: embedReply);
            }
        }

        //List of Rcon commands for PVE Cluster
        //Multiple page embed
        //Not Complete - Needs adding pages with Commands
        [Command("rcon-List")]
        [Description("Shows availible commands for PVE Cluster")]
        public async Task RconCommandList(CommandContext ctx)
        {
            if (ctx.Channel.Id == Config.DiscordOptions.CommandsChannel)
            {
                for (int i = 0; i <= Coms.ComBlock.Count - 1; i++)
                {
                    await ctx.Channel.SendMessageAsync($"``` {Coms.ComBlock[i].Coms}``` ");
                }
            }
        }

        [Command("Commands")]
        [Description("Shows commands to communicate with Bloody Cluster")]
        public async Task BotCommands(CommandContext ctx)
        {
            if (ctx.Channel.Id == Config.DiscordOptions.CommandsChannel)
            {
                var embedReply = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Title = "Server Communication Commands"
                };
                embedReply.WithAuthor("Bloody Command Helper", null, "https://cdn.discordapp.com/attachments/752642174801281214/802311513913950268/large.5897a03b41dc0_ARKHumanIconCompressed.png.323620cc3ef77936cfbdf57eb869579f.png");
                embedReply.AddField("/Rcon", "Used to communicate with rcon", false);
                embedReply.AddField("/Rcon-List", "Lists all availible rcon commands", false);
                embedReply.AddField("/Rcon-Servers", "Shows server ID's", false);


                await ctx.Channel.SendMessageAsync(embed: embedReply);
            }
        }

        [Command("KickMe")]
        [Description("Kicks you from all maps")]
        public async Task KickMe(CommandContext _ctx)
        {
            if (_ctx.Channel.Id != Config.DiscordOptions.CommunityCommandsChannel) return; //returns if not in correct channelS

            string cluster = _ctx.Message.Content.Substring(7).Trim().ToLower();

            if (cluster != "pve" && cluster != "pvp" && cluster != "pvpve")
            {
                await _ctx.RespondAsync("You have not entered the cluster correctly.\n\n Commands:\n/kickme pvpve - kicks you from pvpve cluster\n/kickme pvp - kicks you from pvp cluster" +
                                  "\n/kickme pve - kicks you from the pve cluster");
                return;
            }

            string clusterConstant = cluster;
            string userSteamID = DatabaseManager.GetSteamId(_ctx.User.Id); //gets user steamID from database

            //returns if steam id is not found
            if (userSteamID == "" || userSteamID == "0")
            {
                await _ctx.RespondAsync("You have no linked your account ingame. You must do this before you are able to use this feature");
                return;
            }

            cluster = cluster.ToLower();
            DiscordMessage response;

            switch (cluster)
            {
                case "pvpve":
                    for (int i = 0; i < Config.Servers.Count; i++)
                    {
                        if (Config.Servers[i].ClusterName.Equals("PVPVE"))
                        {
                            cluster = Config.Servers[i].ServerName;
                            string command = $"kickplayer {userSteamID.Trim()}";
                            var respond = await ACM.RconSendCommand(command, cluster);
                        }
                    }
                    response = await _ctx.RespondAsync($"```{_ctx.User.Username} was kicked from the {clusterConstant} cluster```");
                    await Task.Delay(2000);
                    await response.DeleteAsync();
                    break;
                case "pvp":
                    for (int i = 0; i < Config.Servers.Count; i++)
                    {
                        if (Config.Servers[i].ClusterName.Equals("PVP"))
                        {
                            cluster = Config.Servers[i].ServerName;
                            string command = $"kickplayer {userSteamID.Trim()}";
                            var respond = await ACM.RconSendCommand(command, cluster);
                        }
                    }
                    response = await _ctx.RespondAsync($"```{_ctx.User.Username} was kicked from the {clusterConstant} cluster```");
                    await Task.Delay(2000);
                    await response.DeleteAsync();
                    break;
                case "pve":
                    for (int i = 0; i < Config.Servers.Count; i++)
                    {
                        if (Config.Servers[i].ClusterName.Equals("PVE"))
                        {
                            cluster = Config.Servers[i].ServerName;
                            string command = $"kickplayer {userSteamID.Trim()}";
                            var respond = await ACM.RconSendCommand(command, cluster);
                        }
                    }
                    response = await _ctx.RespondAsync($"```{_ctx.User.Username} was kicked from the {clusterConstant} cluster```");
                    await Task.Delay(2000);
                    await response.DeleteAsync();
                    break;
            }
        }

        [Command("lookupDiscord")]
        [Description("finds owner of a steam ID")]
        public async Task LookupDiscord(CommandContext _ctx, ulong _steamId)
        {
            if (_ctx.Channel.Id != Config.DiscordOptions.CommandsChannel) return;

            string discordId = DatabaseManager.GetDiscordId(_steamId);

            if (discordId == "" || discordId == "0")
            {
                await _ctx.RespondAsync("User has not linked their account ingame.");
                return;
            }

            DiscordUser foundUser = await Bot.GetUserAsync(ulong.Parse(discordId));

            DiscordEmbedBuilder userEmbed = new DiscordEmbedBuilder
            {
                Title = foundUser.Username + "#" + foundUser.Discriminator,
                Color = DiscordColor.Red
            };

            userEmbed.WithThumbnail(foundUser.AvatarUrl);

            await _ctx.RespondAsync(userEmbed);
        }

        [Command("lookupSteam")]
        [Description("finds owner of a steam ID")]
        public async Task LookupSteam(CommandContext _ctx)
        {
            if (_ctx.Channel.Id != Config.DiscordOptions.CommandsChannel) return;

            IReadOnlyList<DiscordUser> user = _ctx.Message.MentionedUsers;

            List<string> steamId = user.Select(discordUser => DatabaseManager.GetSteamId(discordUser.Id)).ToList();

            var loopPos = 0;

            foreach (string id in steamId)
            {
                if (id == "" || id == "0")
                {
                    await _ctx.RespondAsync("User has not linked their account ingame.");
                    return;
                }

                DiscordEmbedBuilder userEmbed = new DiscordEmbedBuilder
                {
                    Title = user[loopPos].Username + "#" + user[loopPos].Discriminator,
                    Color = DiscordColor.Red,
                    Description = $"Steam ID = {id}"
                };

                userEmbed.WithThumbnail(user[loopPos].AvatarUrl);

                await _ctx.RespondAsync(userEmbed);

                loopPos++;
            }
        }
    }
}
