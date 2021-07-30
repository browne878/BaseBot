using System;
using System.Collections.Generic;
using System.Text;

namespace ArkDiscordHelper.Models
{
    public class Servers
    {

        public string ClusterName { get; set; }
        public string ServerName { get; set; }
        public string RconIP { get; set; }
        public string RconPass { get; set; }
        public int RconPort { get; set; }
    }
}
