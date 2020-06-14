using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MySqlHelper.IntegrationTests.Configuration
{
    public class ConfigurationSettings
    {
        public ConfigurationSettings()
        {
            var config = new ConfigurationBuilder().AddJsonFile("Configuration/ConfigSettings.json").Build();
            config.Bind(this);
        }

        public string ConnectionString { get; set; }
    }
}
