﻿using System.Threading.Tasks;
using RetroGames.Helpers;
using RetroRoyale.Logic;

namespace RetroRoyale.Protocol.Messages.Server
{
    public class ShutdownStartedMessage : PiranhaMessage
    {
        public ShutdownStartedMessage(Device device) : base(device)
        {
            Id = 20161;
        }

        public override async Task Encode()
        {
            await Stream.WriteInt(0); // SecondsUntilShutdown
        }
    }
}