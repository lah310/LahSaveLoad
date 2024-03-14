using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LahSaveLoad.Configuration
{
    public class LahSaveLoadConfiguration : IRocketPluginConfiguration
    {
        public string Icon { get; set; }
        public bool AutoLoadOnDeath { get; set; }
        public void LoadDefaults()
        {
            AutoLoadOnDeath = true;
            Icon = "url here";
        }

    }
}
