using System;
using System.Collections.Generic;
using System.Text;

namespace ArkDiscordHelper.Models.AlphaEngrams
{
    public class RTA
    {
        public string Name { get; set; }
        public string Cluster { get; set; }
        public string Level { get; set; }
        public List<string> Engrams { get; set; }
    }
}
