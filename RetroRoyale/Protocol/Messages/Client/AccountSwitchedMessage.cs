﻿using System.Threading.Tasks;
using RetroGames.Helpers;
using RetroRoyale.Database;
using RetroRoyale.Logic;

namespace RetroRoyale.Protocol.Messages.Client
{
    public class AccountSwitchedMessage : PiranhaMessage
    {
        public AccountSwitchedMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        public long SwitchedToAccountId { get; set; }

        public override void Decode()
        {
            SwitchedToAccountId = Reader.ReadInt64();
        }

        public override async Task Process()
        {
            await Resources.PlayerCache.RemovePlayer(Device.Player.AccountId, Device.SessionId);
            await PlayerDb.Delete(Device.Player.AccountId);
        }
    }
}