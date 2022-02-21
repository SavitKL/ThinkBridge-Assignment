using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Models.AppSettings
{
    public class AppSettings
    {
        public DatabaseConfiguration DatabaseConfiguration { get; set; }
        public string[] AllowOrigin { get; set; }
        public string BaseRoute { get; set; }
        public bool SwaggerEnabled { get; set; }
        public string ApplicationUrl { get; set; }
        public string AllowedHosts { get; set; }
    }
}
