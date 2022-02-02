using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BeautyDevStat.Tests
{

    internal static class ConfigurationLoader
    {
        public static IConfiguration GetConfiguration(string configPart)
        {
            var basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.{configPart.ToLower()}.json", optional: false, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}