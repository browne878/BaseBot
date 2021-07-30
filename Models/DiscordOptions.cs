using System;
using System.Collections.Generic;
using System.Text;

namespace ArkDiscordHelper.Models
{
   public class DiscordOptions
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public ulong CommandsChannel { get; set; }

        public ulong CommunityCommandsChannel { get; set; }
    }
}
