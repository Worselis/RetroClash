﻿using System;
using System.Threading.Tasks;
using RetroGames.Helpers;
using RetroRoyale.Database;
using RetroRoyale.Logic;

namespace RetroRoyale
{
    public class Program
    {
        private static void Main(string[] args)
        {
            StartAsync().GetAwaiter().GetResult();
        }

        public static async Task StartAsync()
        {
            Console.Clear();

            Console.Title = $"RetroRoyale Server v{Configuration.Version}";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                "________     _____             ________                     ______     \r\n___  __ \\______  /________________  __ \\__________  _______ ___  /____ \r\n__  /_/ /  _ \\  __/_  ___/  __ \\_  /_/ /  __ \\_  / / /  __ `/_  /_  _ \\\r\n_  _, _//  __/ /_ _  /   / /_/ /  _, _// /_/ /  /_/ // /_/ /_  / /  __/\r\n/_/ |_| \\___/\\__/ /_/    \\____//_/ |_| \\____/_\\__, / \\__,_/ /_/  \\___/ \r\n                                             /____/                    ");

            Console.SetOut(new Prefixed());

            Console.WriteLine("Preparing...");
            Console.ResetColor();

            Resources.Construct();

            Console.ResetColor();

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                switch (key)
                {
                    case ConsoleKey.D:
                    {
                        Configuration.Debug = !Configuration.Debug;
                        Console.WriteLine("Debugging has been " + (Configuration.Debug ? "enabled." : "disabled."));
                        break;
                    }

                    case ConsoleKey.E:
                    {
                        Console.WriteLine("Aborting...");

                        await Task.Delay(2000);

                        Environment.Exit(0);
                        break;
                    }

                    case ConsoleKey.G:
                    {
                        Console.WriteLine(
                            $"[GATEWAY] TP: {Resources.Gateway.TokenCount}, BP: {Resources.Gateway.BufferCount}, EP: {Resources.Gateway.EventCount}");
                        break;
                    }

                    case ConsoleKey.H:
                    {
                        Console.WriteLine(
                            "Commands: [D]ebug, [E]xit, [G]ateway, [H]elp, [K]ey, [M]aintenance, [S]tatus");
                        break;
                    }

                    case ConsoleKey.K:
                    {
                        Console.WriteLine($"Generated RC4 Key: {Utils.GenerateRc4Key}");
                        break;
                    }

                    case ConsoleKey.M:
                    {
                        Configuration.Maintenance = !Configuration.Maintenance;

                        if (Configuration.Maintenance)
                            try
                            {
                                Console.WriteLine("Removing every Player in cache...");
                                foreach (var player in Resources.PlayerCache.Values)
                                    player.Device.Disconnect();
                                Resources.PlayerCache.Clear();
                                Console.WriteLine("Done!");
                            }
                            catch (Exception exception)
                            {
                                Logger.Log(exception, Enums.LogType.Error);
                            }

                        Console.WriteLine("Maintenance has been " +
                                          (Configuration.Maintenance ? "enabled." : "disabled."));
                        break;
                    }

                    case ConsoleKey.S:
                    {
                        Console.WriteLine(
                            $"[STATUS] Online Players: {Resources.PlayerCache.Count}, Connected Sockets: {Resources.Gateway.ConnectedSockets}, Players Saved: {await PlayerDb.PlayerCount()}, Cached Players: {(Redis.IsConnected ? Redis.CachedPlayers() : 0)}");
                        break;
                    }

                    default:
                    {
                        Console.WriteLine("Invalid Key. Press 'H' for help.");
                        break;
                    }
                }

                Console.ResetColor();
            }
        }
    }
}