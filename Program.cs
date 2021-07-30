using ArkDiscordHelper.Commands;
using ArkDiscordHelper.Models;
using ArkDiscordHelper.Models.AlphaEngrams;
using ArkDiscordHelper.Services;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArkDiscordHelper
{
    public class Program
    {
        private DiscordClient Bot;
        private CommandsNextExtension Commands;
        private FileService FileManager;
        private Config Config;
        private ComBlocks ComBlocks;
        private RoadToAlphas RoadToAlphas;

        static void Main(string[] args)
        {
            var program = new Program();
            program.MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            FileManager = new FileService();
            Config = FileManager.GetConfig();
            ComBlocks = FileManager.GetComBlocks();
            RoadToAlphas = FileManager.GetRTA();
            //bot setup configuration
            Bot = new DiscordClient(new DiscordConfiguration()
            {
                Token = Config.DiscordOptions.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            });

            var services = ConfigureServices();

            //await services.GetRequiredService<ArkCommandManager>().InitializeAsync();
            //commands setup configuration
            var ccfg = new CommandsNextConfiguration
            {
                StringPrefixes = new[] { Config.DiscordOptions.Prefix },
                EnableDms = true,
                EnableMentionPrefix = true,
                Services = services,
                EnableDefaultHelp = false,
                IgnoreExtraArguments = true,
            };

            //Set up interactivity
            Bot.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromSeconds(30)
            });

            Commands = Bot.UseCommandsNext(ccfg);

            //register commands
            Commands.RegisterCommands<RconCommands>();

         

            //login
            await Bot.ConnectAsync();
            await Task.Delay(Timeout.Infinite);

        }


        //dependency injection
        public IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<FileService>()
                .AddSingleton<ArkCommandManager>()
                .AddSingleton<RconManager>()
                .AddSingleton<DatabaseManager>()
                .AddSingleton(Bot)
                .AddSingleton(Config)
                .AddSingleton(ComBlocks)
                .AddSingleton(RoadToAlphas)
                .BuildServiceProvider();
        }
    }
}
