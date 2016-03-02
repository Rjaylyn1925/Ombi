﻿using System;
using System.Diagnostics;
using Microsoft.Owin.Hosting;

using Mono.Data.Sqlite;

using RequestPlex.Core;
using RequestPlex.Core.SettingModels;
using RequestPlex.Helpers;
using RequestPlex.Store;
using RequestPlex.Store.Repository.NZBDash.DataAccessLayer.Repository;

namespace RequestPlex.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyVer = AssemblyHelper.GetAssemblyVersion();
            Console.WriteLine($"Version: {assemblyVer}");
            var uri = "http://localhost:3579/";
            var s = new Setup();
            s.SetupDb();

            var service = new SettingsServiceV2<RequestPlexSettings>(new JsonRepository(new DbConfiguration(new SqliteFactory()), new MemoryCacheProvider()));
            var settings = service.GetSettings();

            if (settings.Port != 0)
            {
                uri = $"http://localhost:{settings.Port}";
            }

            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine($"Request Plex is running on {uri}");
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
        }
    }
}