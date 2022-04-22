﻿using RSBot.Core.Network;

namespace LuckyAlchemyBot.Network.Handler
{
    internal class StoneRequestHandler : IPacketHandler
    {
        #region Properties

        public ushort Opcode => 0x7151;

        public PacketDestination Destination => PacketDestination.Server;

        #endregion Properties

        #region Methods

        public void Invoke(Packet packet)
        {
            FuseRequestHelper.Invoke(packet);
        }

        #endregion Methods
    }
}